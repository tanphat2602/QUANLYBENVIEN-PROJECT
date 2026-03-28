using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBenhVien.API.Data;
using QuanLyBenhVien.API.Models;

namespace QuanLyBenhVien.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchedulesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SchedulesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var schedules = _context.DoctorSchedules
            .Include(s => s.Doctor).ThenInclude(d => d!.User)
            .ToList()
            .Select(s => new
            {
                s.ScheduleID,
                s.DoctorID,
                DoctorName = s.Doctor?.User?.FullName,
                s.DayOfWeek,
                s.Shift,
                s.TimeStart,
                s.TimeEnd,
                s.MaxPatients,
                s.CurrentPatients,
                s.Status
            });

        return Ok(schedules);
    }

    [HttpGet("doctor/{doctorId}")]
    public IActionResult GetByDoctor(int doctorId)
    {
        var schedules = _context.DoctorSchedules
            .Where(s => s.DoctorID == doctorId)
            .ToList()
            .Select(s => new
            {
                s.ScheduleID,
                s.DayOfWeek,
                s.Shift,
                s.TimeStart,
                s.TimeEnd,
                s.MaxPatients,
                s.CurrentPatients,
                s.Status
            });

        return Ok(schedules);
    }

    [HttpGet("available")]
    public IActionResult GetAvailable(string date, int departmentId)
    {
        var dayOfWeek = GetDayOfWeek(date);
        var schedules = _context.DoctorSchedules
            .Include(s => s.Doctor).ThenInclude(d => d!.User)
            .Include(s => s.Doctor).ThenInclude(d => d!.Department)
            .Where(s => s.DayOfWeek == dayOfWeek 
                && s.Doctor!.DepartmentID == departmentId 
                && s.Status == "Hoạt động"
                && s.CurrentPatients < s.MaxPatients)
            .ToList()
            .Select(s => new
            {
                s.ScheduleID,
                s.DoctorID,
                DoctorName = s.Doctor?.User?.FullName,
                Specialty = s.Doctor?.Specialty,
                s.Shift,
                s.TimeStart,
                s.TimeEnd,
                AvailableSlots = s.MaxPatients - s.CurrentPatients
            });

        return Ok(schedules);
    }

    private string GetDayOfWeek(string date)
    {
        if (DateTime.TryParse(date, out var d))
        {
            return d.DayOfWeek switch
            {
                DayOfWeek.Monday => "Thứ 2",
                DayOfWeek.Tuesday => "Thứ 3",
                DayOfWeek.Wednesday => "Thứ 4",
                DayOfWeek.Thursday => "Thứ 5",
                DayOfWeek.Friday => "Thứ 6",
                DayOfWeek.Saturday => "Thứ 7",
                DayOfWeek.Sunday => "Chủ nhật",
                _ => ""
            };
        }
        return "";
    }

    [HttpPost]
    public IActionResult Create([FromBody] ScheduleCreateRequest request)
    {
        var schedule = new DoctorSchedule
        {
            DoctorID = request.DoctorID,
            DayOfWeek = request.DayOfWeek,
            Shift = request.Shift,
            TimeStart = request.TimeStart,
            TimeEnd = request.TimeEnd,
            MaxPatients = request.MaxPatients,
            CurrentPatients = 0,
            Status = "Hoạt động"
        };

        _context.DoctorSchedules.Add(schedule);
        _context.SaveChanges();

        return Ok(new { message = "Thêm lịch thành công!", scheduleId = schedule.ScheduleID });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ScheduleUpdateRequest request)
    {
        var schedule = _context.DoctorSchedules.Find(id);
        if (schedule == null)
            return NotFound();

        if (request.DayOfWeek != null)
            schedule.DayOfWeek = request.DayOfWeek;
        if (request.Shift != null)
            schedule.Shift = request.Shift;
        if (request.TimeStart != null)
            schedule.TimeStart = request.TimeStart;
        if (request.TimeEnd != null)
            schedule.TimeEnd = request.TimeEnd;
        if (request.MaxPatients.HasValue)
            schedule.MaxPatients = request.MaxPatients;
        if (request.Status != null)
            schedule.Status = request.Status;

        _context.SaveChanges();
        return Ok(new { message = "Cập nhật thành công!" });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var schedule = _context.DoctorSchedules.Find(id);
        if (schedule == null)
            return NotFound();

        _context.DoctorSchedules.Remove(schedule);
        _context.SaveChanges();

        return Ok(new { message = "Xóa lịch thành công!" });
    }
}

public class ScheduleCreateRequest
{
    public int? DoctorID { get; set; }
    public string? DayOfWeek { get; set; }
    public string? Shift { get; set; }
    public string? TimeStart { get; set; }
    public string? TimeEnd { get; set; }
    public int? MaxPatients { get; set; }
}

public class ScheduleUpdateRequest
{
    public string? DayOfWeek { get; set; }
    public string? Shift { get; set; }
    public string? TimeStart { get; set; }
    public string? TimeEnd { get; set; }
    public int? MaxPatients { get; set; }
    public string? Status { get; set; }
}
