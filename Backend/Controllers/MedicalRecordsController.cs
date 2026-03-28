using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBenhVien.API.Data;
using QuanLyBenhVien.API.Models;

namespace QuanLyBenhVien.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicalRecordsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MedicalRecordsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var records = _context.MedicalRecords
            .Include(m => m.Patient).ThenInclude(p => p!.User)
            .Include(m => m.Doctor).ThenInclude(d => d!.User)
            .Include(m => m.Appointment)
            .ToList()
            .Select(m => new
            {
                m.RecordID,
                m.PatientID,
                PatientName = m.Patient?.User?.FullName,
                m.DoctorID,
                DoctorName = m.Doctor?.User?.FullName,
                m.AppointmentID,
                m.Symptoms,
                m.Diagnosis,
                m.Treatment,
                m.Notes,
                m.VitalSigns,
                m.Status,
                m.DateCreated
            });

        return Ok(records);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var record = _context.MedicalRecords
            .Include(m => m.Patient).ThenInclude(p => p!.User)
            .Include(m => m.Doctor).ThenInclude(d => d!.User)
            .Include(m => m.Prescriptions).ThenInclude(p => p.Details).ThenInclude(d => d!.Medicine)
            .Include(m => m.TestRequests)
            .FirstOrDefault(m => m.RecordID == id);

        if (record == null)
            return NotFound();

        return Ok(new
        {
            record.RecordID,
            record.PatientID,
            PatientName = record.Patient?.User?.FullName,
            record.DoctorID,
            DoctorName = record.Doctor?.User?.FullName,
            record.Symptoms,
            record.Diagnosis,
            record.Treatment,
            record.Notes,
            record.VitalSigns,
            record.Status,
            record.DateCreated,
            Prescriptions = record.Prescriptions.Select(p => new
            {
                p.PrescriptionID,
                p.Diagnosis,
                p.Status,
                Details = p.Details.Select(d => new
                {
                    d.DetailID,
                    MedicineName = d.Medicine?.MedicineName,
                    d.Dosage,
                    d.Quantity,
                    d.UsageInstructions,
                    d.Duration
                })
            }),
            TestRequests = record.TestRequests.Select(t => new
            {
                t.TestID,
                t.TestType,
                t.Description,
                t.Status,
                t.Result,
                t.RequestDate
            })
        });
    }

    [HttpGet("patient/{patientId}")]
    public IActionResult GetByPatient(int patientId)
    {
        var records = _context.MedicalRecords
            .Include(m => m.Doctor).ThenInclude(d => d!.User)
            .Where(m => m.PatientID == patientId)
            .OrderByDescending(m => m.DateCreated)
            .ToList()
            .Select(m => new
            {
                m.RecordID,
                DoctorName = m.Doctor?.User?.FullName,
                m.Diagnosis,
                m.DateCreated,
                m.Status
            });

        return Ok(records);
    }

    [HttpPost]
    public IActionResult Create([FromBody] MedicalRecordCreateRequest request)
    {
        var record = new MedicalRecord
        {
            PatientID = request.PatientID,
            DoctorID = request.DoctorID,
            AppointmentID = request.AppointmentID,
            Symptoms = request.Symptoms,
            Diagnosis = request.Diagnosis,
            Treatment = request.Treatment,
            Notes = request.Notes,
            VitalSigns = request.VitalSigns,
            Status = "Hoàn thành",
            DateCreated = DateTime.Now
        };

        _context.MedicalRecords.Add(record);
        _context.SaveChanges();

        return Ok(new { message = "Tạo hồ sơ bệnh án thành công!", recordId = record.RecordID });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] MedicalRecordUpdateRequest request)
    {
        var record = _context.MedicalRecords.Find(id);
        if (record == null)
            return NotFound();

        if (request.Symptoms != null)
            record.Symptoms = request.Symptoms;
        if (request.Diagnosis != null)
            record.Diagnosis = request.Diagnosis;
        if (request.Treatment != null)
            record.Treatment = request.Treatment;
        if (request.Notes != null)
            record.Notes = request.Notes;
        if (request.VitalSigns != null)
            record.VitalSigns = request.VitalSigns;

        record.UpdatedAt = DateTime.Now;
        _context.SaveChanges();

        return Ok(new { message = "Cập nhật hồ sơ bệnh án thành công!" });
    }
}

public class MedicalRecordCreateRequest
{
    public int? PatientID { get; set; }
    public int? DoctorID { get; set; }
    public int? AppointmentID { get; set; }
    public string? Symptoms { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Notes { get; set; }
    public string? VitalSigns { get; set; }
}

public class MedicalRecordUpdateRequest
{
    public string? Symptoms { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Notes { get; set; }
    public string? VitalSigns { get; set; }
}
