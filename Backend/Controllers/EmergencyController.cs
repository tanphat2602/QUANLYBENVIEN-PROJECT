using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBenhVien.API.Data;
using QuanLyBenhVien.API.Models;

namespace QuanLyBenhVien.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmergencyController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EmergencyController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var emergencies = _context.EmergencyRequests
            .Include(e => e.Patient).ThenInclude(p => p!.User)
            .Include(e => e.AssignedDoctor).ThenInclude(d => d!.User)
            .ToList()
            .Select(e => new
            {
                e.EmergencyID,
                e.PatientID,
                PatientName = e.Patient?.User?.FullName,
                Phone = e.Patient?.User?.Phone,
                e.AssignedDoctorID,
                DoctorName = e.AssignedDoctor?.User?.FullName,
                e.Reason,
                e.Symptoms,
                e.Status,
                e.Priority,
                e.RequestedAt,
                e.AssignedAt,
                e.CompletedAt
            });

        return Ok(emergencies);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var emergency = _context.EmergencyRequests
            .Include(e => e.Patient).ThenInclude(p => p!.User)
            .Include(e => e.AssignedDoctor).ThenInclude(d => d!.User)
            .FirstOrDefault(e => e.EmergencyID == id);

        if (emergency == null)
            return NotFound();

        return Ok(new
        {
            emergency.EmergencyID,
            emergency.PatientID,
            PatientName = emergency.Patient?.User?.FullName,
            emergency.AssignedDoctorID,
            DoctorName = emergency.AssignedDoctor?.User?.FullName,
            emergency.Reason,
            emergency.Symptoms,
            emergency.Status,
            emergency.Priority,
            emergency.RequestedAt
        });
    }

    [HttpGet("pending")]
    public IActionResult GetPending()
    {
        var emergencies = _context.EmergencyRequests
            .Include(e => e.Patient).ThenInclude(p => p!.User)
            .Where(e => e.Status == "Đang chờ")
            .OrderByDescending(e => e.Priority)
            .ThenBy(e => e.RequestedAt)
            .ToList()
            .Select(e => new
            {
                e.EmergencyID,
                PatientName = e.Patient?.User?.FullName,
                Phone = e.Patient?.User?.Phone,
                e.Symptoms,
                e.Priority,
                e.RequestedAt
            });

        return Ok(emergencies);
    }

    [HttpPost]
    public IActionResult Create([FromBody] EmergencyCreateRequest request)
    {
        var emergency = new EmergencyRequest
        {
            PatientID = request.PatientID,
            Reason = request.Reason,
            Symptoms = request.Symptoms,
            Status = "Đang chờ",
            Priority = request.Priority?.ToString() ?? "1",
            RequestedAt = DateTime.Now
        };

        _context.EmergencyRequests.Add(emergency);
        _context.SaveChanges();

        return Ok(new { message = "Yêu cầu cấp cứu đã được gửi!", emergencyId = emergency.EmergencyID });
    }

    [HttpPut("{id}/assign")]
    public IActionResult AssignDoctor(int id, [FromBody] EmergencyAssignRequest request)
    {
        var emergency = _context.EmergencyRequests.Find(id);
        if (emergency == null)
            return NotFound();

        emergency.AssignedDoctorID = request.DoctorID;
        emergency.AssignedAt = DateTime.Now;
        emergency.Status = "Đang xử lý";

        _context.SaveChanges();
        return Ok(new { message = "Đã gán bác sĩ thành công!" });
    }

    [HttpPut("{id}/complete")]
    public IActionResult Complete(int id)
    {
        var emergency = _context.EmergencyRequests.Find(id);
        if (emergency == null)
            return NotFound();

        emergency.Status = "Hoàn thành";
        emergency.CompletedAt = DateTime.Now;

        _context.SaveChanges();
        return Ok(new { message = "Cấp cứu hoàn thành!" });
    }
}

public class EmergencyCreateRequest
{
    public int? PatientID { get; set; }
    public string? Reason { get; set; }
    public string? Symptoms { get; set; }
    public int? Priority { get; set; }
}

public class EmergencyAssignRequest
{
    public int? DoctorID { get; set; }
}
