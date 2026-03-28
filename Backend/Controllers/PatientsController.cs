using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBenhVien.API.Data;
using QuanLyBenhVien.API.Models;

namespace QuanLyBenhVien.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PatientsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var patients = _context.Patients
            .Include(p => p.User)
            .Include(p => p.Insurance)
            .ToList()
            .Select(p => new
            {
                p.PatientID,
                p.UserID,
                FullName = p.User?.FullName,
                Email = p.User?.Email,
                Phone = p.User?.Phone,
                Gender = p.User?.Gender,
                DateOfBirth = p.User?.DateOfBirth,
                Address = p.User?.Address,
                BloodType = p.BloodType,
                Allergies = p.Allergies,
                Height = p.Height,
                Weight = p.Weight,
                Status = p.Status,
                InsuranceProvider = p.Insurance?.InsuranceProvider,
                PolicyNumber = p.Insurance?.PolicyNumber,
                ExpiryDate = p.Insurance?.ExpiryDate
            })
            .ToList();

        return Ok(patients);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var patient = _context.Patients
            .Include(p => p.User)
            .Include(p => p.Insurance)
            .FirstOrDefault(p => p.PatientID == id);

        if (patient == null)
            return NotFound();

        return Ok(new
        {
            patient.PatientID,
            patient.UserID,
            FullName = patient.User?.FullName,
            Email = patient.User?.Email,
            Phone = patient.User?.Phone,
            Gender = patient.User?.Gender,
            DateOfBirth = patient.User?.DateOfBirth,
            Address = patient.User?.Address,
            BloodType = patient.BloodType,
            Allergies = patient.Allergies,
            Height = patient.Height,
            Weight = patient.Weight,
            Status = patient.Status,
            InsuranceProvider = patient.Insurance?.InsuranceProvider,
            PolicyNumber = patient.Insurance?.PolicyNumber,
            ExpiryDate = patient.Insurance?.ExpiryDate
        });
    }

    [HttpGet("user/{userId}")]
    public IActionResult GetByUserId(int userId)
    {
        var patient = _context.Patients
            .Include(p => p.User)
            .Include(p => p.Insurance)
            .FirstOrDefault(p => p.UserID == userId);

        if (patient == null)
            return NotFound();

        return Ok(new
        {
            patient.PatientID,
            patient.UserID,
            FullName = patient.User?.FullName,
            Email = patient.User?.Email,
            Phone = patient.User?.Phone,
            Address = patient.User?.Address,
            Status = patient.Status
        });
    }

    [HttpPost]
    public IActionResult Create([FromBody] PatientCreateRequest request)
    {
        var patient = new Patient
        {
            UserID = request.UserID,
            BloodType = request.BloodType,
            Allergies = request.Allergies,
            Height = request.Height,
            Weight = request.Weight,
            InsuranceID = request.InsuranceID,
            Status = "Hoạt động",
            CreatedAt = DateTime.Now
        };

        _context.Patients.Add(patient);
        _context.SaveChanges();

        return Ok(new { message = "Thêm bệnh nhân thành công!", patientId = patient.PatientID });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] PatientUpdateRequest request)
    {
        var patient = _context.Patients.Find(id);
        if (patient == null)
            return NotFound();

        if (request.BloodType != null)
            patient.BloodType = request.BloodType;
        if (request.Allergies != null)
            patient.Allergies = request.Allergies;
        if (request.Height.HasValue)
            patient.Height = request.Height;
        if (request.Weight.HasValue)
            patient.Weight = request.Weight;
        if (request.InsuranceID.HasValue)
            patient.InsuranceID = request.InsuranceID;
        if (request.Status != null)
            patient.Status = request.Status;

        _context.SaveChanges();
        return Ok(new { message = "Cập nhật thành công!" });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var patient = _context.Patients.Find(id);
        if (patient == null)
            return NotFound();

        _context.Patients.Remove(patient);
        _context.SaveChanges();

        return Ok(new { message = "Xóa thành công!" });
    }
}

public class PatientCreateRequest
{
    public int? UserID { get; set; }
    public string? BloodType { get; set; }
    public string? Allergies { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    public int? InsuranceID { get; set; }
}

public class PatientUpdateRequest
{
    public string? BloodType { get; set; }
    public string? Allergies { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    public int? InsuranceID { get; set; }
    public string? Status { get; set; }
}
