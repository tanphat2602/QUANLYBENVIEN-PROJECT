using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBenhVien.API.Data;
using QuanLyBenhVien.API.Models;

namespace QuanLyBenhVien.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PaymentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var payments = _context.Payments
            .Include(p => p.Patient).ThenInclude(p => p!.User)
            .Include(p => p.Appointment)
            .ToList()
            .Select(p => new
            {
                p.PaymentID,
                p.PatientID,
                PatientName = p.Patient?.User?.FullName,
                p.AppointmentID,
                p.Amount,
                p.PaymentMethod,
                p.Status,
                p.TransactionID,
                p.PaymentDate,
                p.Description
            });

        return Ok(payments);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var payment = _context.Payments
            .Include(p => p.Patient).ThenInclude(p => p!.User)
            .FirstOrDefault(p => p.PaymentID == id);

        if (payment == null)
            return NotFound();

        return Ok(new
        {
            payment.PaymentID,
            payment.PatientID,
            PatientName = payment.Patient?.User?.FullName,
            payment.Amount,
            payment.PaymentMethod,
            payment.Status,
            payment.TransactionID,
            payment.PaymentDate,
            payment.Description
        });
    }

    [HttpGet("patient/{patientId}")]
    public IActionResult GetByPatient(int patientId)
    {
        var payments = _context.Payments
            .Where(p => p.PatientID == patientId)
            .OrderByDescending(p => p.PaymentDate)
            .ToList()
            .Select(p => new
            {
                p.PaymentID,
                p.Amount,
                p.PaymentMethod,
                p.Status,
                p.PaymentDate,
                p.Description
            });

        return Ok(payments);
    }

    [HttpPost]
    public IActionResult Create([FromBody] PaymentCreateRequest request)
    {
        var payment = new Payment
        {
            PatientID = request.PatientID,
            AppointmentID = request.AppointmentID,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            Status = "Hoàn thành",
            TransactionID = GenerateTransactionId(),
            PaymentDate = DateTime.Now,
            Description = request.Description
        };

        _context.Payments.Add(payment);
        _context.SaveChanges();

        return Ok(new { message = "Thanh toán thành công!", paymentId = payment.PaymentID, transactionId = payment.TransactionID });
    }

    [HttpPut("{id}/refund")]
    public IActionResult Refund(int id)
    {
        var payment = _context.Payments.Find(id);
        if (payment == null)
            return NotFound();

        payment.Status = "Đã hoàn tiền";
        _context.SaveChanges();

        return Ok(new { message = "Hoàn tiền thành công!" });
    }

    private string GenerateTransactionId()
    {
        return "TXN" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);
    }
}

public class PaymentCreateRequest
{
    public int? PatientID { get; set; }
    public int? AppointmentID { get; set; }
    public decimal? Amount { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Description { get; set; }
}
