using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBenhVien.API.Data;
using QuanLyBenhVien.API.Models;

namespace QuanLyBenhVien.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AppointmentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var appointments = _context.Appointments
            .Include(a => a.Patient)
            .ThenInclude(p => p!.User)
            .Include(a => a.Doctor)
            .ThenInclude(d => d!.User)
            .Select(a => new
            {
                a.AppointmentID,
                a.PatientID,
                a.DoctorID,
                PatientName = a.Patient != null && a.Patient.User != null ? a.Patient.User.FullName : null,
                DoctorName = a.Doctor != null && a.Doctor.User != null ? a.Doctor.User.FullName : null,
                a.AppointmentDate,
                a.Status,
                a.Notes
            })
            .ToList();

        return Ok(appointments);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var appointment = _context.Appointments
            .Include(a => a.Patient)
            .ThenInclude(p => p!.User)
            .Include(a => a.Doctor)
            .ThenInclude(d => d!.User)
            .FirstOrDefault(a => a.AppointmentID == id);

        if (appointment == null)
            return NotFound();

        return Ok(new
        {
            appointment.AppointmentID,
            appointment.PatientID,
            appointment.DoctorID,
            PatientName = appointment.Patient?.User?.FullName,
            DoctorName = appointment.Doctor?.User?.FullName,
            appointment.AppointmentDate,
            appointment.Status,
            appointment.Notes
        });
    }

    [HttpGet("doctor/{doctorId}")]
    public IActionResult GetByDoctor(int doctorId)
    {
        var appointments = _context.Appointments
            .Include(a => a.Patient)
            .ThenInclude(p => p!.User)
            .Where(a => a.DoctorID == doctorId)
            .Select(a => new
            {
                a.AppointmentID,
                a.PatientID,
                PatientName = a.Patient != null && a.Patient.User != null ? a.Patient.User.FullName : null,
                a.AppointmentDate,
                a.Status,
                a.Notes
            })
            .ToList();

        return Ok(appointments);
    }

    [HttpPost]
    public IActionResult Create([FromBody] AppointmentCreateRequest request)
    {
        var appointment = new Appointment
        {
            PatientID = request.PatientID,
            DoctorID = request.DoctorID,
            AppointmentDate = request.AppointmentDate,
            Status = request.Status ?? "Chờ xác nhận",
            Notes = request.Notes
        };

        _context.Appointments.Add(appointment);
        _context.SaveChanges();

        return Ok(new { message = "Tạo lịch hẹn thành công!", appointmentId = appointment.AppointmentID });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] AppointmentUpdateRequest request)
    {
        var appointment = _context.Appointments.Find(id);
        if (appointment == null)
            return NotFound();

        if (request.AppointmentDate.HasValue)
            appointment.AppointmentDate = request.AppointmentDate;
        if (request.Status != null)
            appointment.Status = request.Status;
        if (request.Notes != null)
            appointment.Notes = request.Notes;

        _context.SaveChanges();
        return Ok(new { message = "Cập nhật thành công!" });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var appointment = _context.Appointments.Find(id);
        if (appointment == null)
            return NotFound();

        _context.Appointments.Remove(appointment);
        _context.SaveChanges();

        return Ok(new { message = "Xóa thành công!" });
    }
}

public class AppointmentCreateRequest
{
    public int? PatientID { get; set; }
    public int? DoctorID { get; set; }
    public DateTime? AppointmentDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
}

public class AppointmentUpdateRequest
{
    public DateTime? AppointmentDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
}
