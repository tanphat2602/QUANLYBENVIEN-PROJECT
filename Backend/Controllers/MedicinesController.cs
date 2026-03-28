using Microsoft.AspNetCore.Mvc;
using QuanLyBenhVien.API.Data;
using QuanLyBenhVien.API.Models;

namespace QuanLyBenhVien.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicinesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MedicinesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var medicines = _context.Medicines.ToList();
        return Ok(medicines);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var medicine = _context.Medicines.Find(id);
        if (medicine == null)
            return NotFound();
        return Ok(medicine);
    }

    [HttpGet("search/{keyword}")]
    public IActionResult Search(string keyword)
    {
        var medicines = _context.Medicines
            .Where(m => (m.MedicineName != null && m.MedicineName.Contains(keyword)) || 
                        (m.GenericName != null && m.GenericName.Contains(keyword)))
            .ToList();
        return Ok(medicines);
    }

    [HttpPost]
    public IActionResult Create([FromBody] MedicineCreateRequest request)
    {
        var medicine = new Medicine
        {
            MedicineName = request.MedicineName,
            GenericName = request.GenericName,
            Manufacturer = request.Manufacturer,
            Unit = request.Unit,
            Price = request.Price,
            Stock = request.Stock,
            DosageForm = request.DosageForm,
            ExpiryDate = request.ExpiryDate,
            Status = "Hoạt động"
        };

        _context.Medicines.Add(medicine);
        _context.SaveChanges();

        return Ok(new { message = "Thêm thuốc thành công!", medicineId = medicine.MedicineID });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] MedicineUpdateRequest request)
    {
        var medicine = _context.Medicines.Find(id);
        if (medicine == null)
            return NotFound();

        if (request.MedicineName != null)
            medicine.MedicineName = request.MedicineName;
        if (request.GenericName != null)
            medicine.GenericName = request.GenericName;
        if (request.Manufacturer != null)
            medicine.Manufacturer = request.Manufacturer;
        if (request.Unit != null)
            medicine.Unit = request.Unit;
        if (request.Price.HasValue)
            medicine.Price = request.Price;
        if (request.Stock.HasValue)
            medicine.Stock = request.Stock;
        if (request.DosageForm != null)
            medicine.DosageForm = request.DosageForm;
        if (request.ExpiryDate.HasValue)
            medicine.ExpiryDate = request.ExpiryDate;
        if (request.Status != null)
            medicine.Status = request.Status;

        _context.SaveChanges();
        return Ok(new { message = "Cập nhật thành công!" });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var medicine = _context.Medicines.Find(id);
        if (medicine == null)
            return NotFound();

        _context.Medicines.Remove(medicine);
        _context.SaveChanges();

        return Ok(new { message = "Xóa thuốc thành công!" });
    }
}

public class MedicineCreateRequest
{
    public string MedicineName { get; set; } = string.Empty;
    public string? GenericName { get; set; }
    public string? Manufacturer { get; set; }
    public string? Unit { get; set; }
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
    public string? DosageForm { get; set; }
    public DateTime? ExpiryDate { get; set; }
}

public class MedicineUpdateRequest
{
    public string? MedicineName { get; set; }
    public string? GenericName { get; set; }
    public string? Manufacturer { get; set; }
    public string? Unit { get; set; }
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
    public string? DosageForm { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Status { get; set; }
}
