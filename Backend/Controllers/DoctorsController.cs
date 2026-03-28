using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBenhVien.API.Data;
using QuanLyBenhVien.API.Models;

namespace QuanLyBenhVien.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DoctorsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var doctors = _context.Doctors
            .Include(d => d.User)
            .Include(d => d.Department)
            .Select(d => new
            {
                d.DoctorID,
                d.UserID,
                FullName = d.User != null ? d.User.FullName : null,
                Email = d.User != null ? d.User.Email : null,
                Phone = d.User != null ? d.User.Phone : null,
                Specialty = d.Specialty,
                WorkingSchedule = d.WorkingSchedule,
                DepartmentName = d.Department != null ? d.Department.DepartmentName : null
            })
            .ToList();

        return Ok(doctors);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var doctor = _context.Doctors
            .Include(d => d.User)
            .Include(d => d.Department)
            .FirstOrDefault(d => d.DoctorID == id);

        if (doctor == null)
            return NotFound();

        return Ok(new
        {
            doctor.DoctorID,
            doctor.UserID,
            FullName = doctor.User?.FullName,
            Email = doctor.User?.Email,
            Phone = doctor.User?.Phone,
            doctor.Specialty,
            doctor.WorkingSchedule,
            DepartmentName = doctor.Department?.DepartmentName
        });
    }

    [HttpGet("user/{userId}")]
    public IActionResult GetByUserId(int userId)
    {
        var doctor = _context.Doctors
            .Include(d => d.User)
            .Include(d => d.Department)
            .FirstOrDefault(d => d.UserID == userId);

        if (doctor == null)
            return NotFound();

        return Ok(new
        {
            doctor.DoctorID,
            doctor.UserID,
            FullName = doctor.User?.FullName,
            Email = doctor.User?.Email,
            doctor.Specialty,
            doctor.WorkingSchedule,
            DepartmentName = doctor.Department?.DepartmentName
        });
    }

    [HttpPost]
    public IActionResult Create([FromBody] DoctorCreateRequest request)
    {
        var doctor = new Doctor
        {
            UserID = request.UserID,
            DepartmentID = request.DepartmentID,
            Specialty = request.Specialty,
            WorkingSchedule = request.WorkingSchedule
        };

        _context.Doctors.Add(doctor);
        _context.SaveChanges();

        return Ok(new { message = "Thêm bác sĩ thành công!", doctorId = doctor.DoctorID });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] DoctorUpdateRequest request)
    {
        var doctor = _context.Doctors.Find(id);
        if (doctor == null)
            return NotFound();

        if (request.DepartmentID.HasValue)
            doctor.DepartmentID = request.DepartmentID;
        if (request.Specialty != null)
            doctor.Specialty = request.Specialty;
        if (request.WorkingSchedule != null)
            doctor.WorkingSchedule = request.WorkingSchedule;

        _context.SaveChanges();
        return Ok(new { message = "Cập nhật thành công!" });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var doctor = _context.Doctors.Find(id);
        if (doctor == null)
            return NotFound();

        _context.Doctors.Remove(doctor);
        _context.SaveChanges();

        return Ok(new { message = "Xóa thành công!" });
    }
}

public class DoctorCreateRequest
{
    public int? UserID { get; set; }
    public int? DepartmentID { get; set; }
    public string? Specialty { get; set; }
    public string? WorkingSchedule { get; set; }
}

public class DoctorUpdateRequest
{
    public int? DepartmentID { get; set; }
    public string? Specialty { get; set; }
    public string? WorkingSchedule { get; set; }
}
