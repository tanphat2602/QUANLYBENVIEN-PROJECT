using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBenhVien.API.Data;
using QuanLyBenhVien.API.Models;

namespace QuanLyBenhVien.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("stats")]
    public IActionResult GetStats()
    {
        var today = DateTime.Today;
        var totalUsers = _context.Users.Count();
        var totalDoctors = _context.Doctors.Count();
        var totalPatients = _context.Patients.Count();
        var totalAppointments = _context.Appointments.Count();
        var totalRevenue = _context.Payments.Where(p => p.Status == "Hoàn thành").Sum(p => p.Amount) ?? 0;
        var totalDepartments = _context.Departments.Count();
        var todayAppointments = _context.Appointments.Count(a => a.AppointmentDate == today);
        var emergencyRequests = _context.EmergencyRequests.Count(e => e.Status == "Đang chờ");
        var doctorsOnDuty = _context.Doctors.Count(d => d.Status == "Hoạt động");

        return Ok(new
        {
            totalUsers,
            totalDoctors,
            totalPatients,
            totalAppointments,
            totalRevenue,
            totalDepartments,
            todayAppointments,
            emergencyRequests,
            doctorsOnDuty
        });
    }

    [HttpGet("today")]
    public IActionResult GetTodayStats()
    {
        var today = DateTime.Today;
        var appointments = _context.Appointments
            .Include(a => a.Patient).ThenInclude(p => p!.User)
            .Include(a => a.Doctor).ThenInclude(d => d!.User)
            .Where(a => a.AppointmentDate == today)
            .OrderBy(a => a.TimeSlot)
            .ToList()
            .Select(a => new
            {
                a.AppointmentID,
                PatientName = a.Patient?.User?.FullName,
                DoctorName = a.Doctor?.User?.FullName,
                a.TimeSlot,
                a.Status,
                a.IsEmergency
            });

        return Ok(appointments);
    }

    [HttpGet("recent-patients")]
    public IActionResult GetRecentPatients()
    {
        var patients = _context.Patients
            .Include(p => p.User)
            .OrderByDescending(p => p.CreatedAt)
            .Take(10)
            .ToList()
            .Select(p => new
            {
                p.PatientID,
                FullName = p.User?.FullName,
                Phone = p.User?.Phone,
                Status = p.Status,
                CreatedAt = p.CreatedAt
            });

        return Ok(patients);
    }
}
