using Microsoft.AspNetCore.Mvc;
using QuanLyBenhVien.API.Data;
using QuanLyBenhVien.API.Models;

namespace QuanLyBenhVien.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ServicesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var services = _context.MedicalServices.ToList();
        return Ok(services);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var service = _context.MedicalServices.Find(id);
        if (service == null)
            return NotFound();
        return Ok(service);
    }

    [HttpPost]
    public IActionResult Create([FromBody] MedicalServiceCreateRequest request)
    {
        var service = new MedicalService
        {
            ServiceName = request.ServiceName,
            Description = request.Description,
            Price = request.Price,
            DepartmentID = request.DepartmentID,
            Status = "Hoạt động"
        };

        _context.MedicalServices.Add(service);
        _context.SaveChanges();

        return Ok(new { message = "Thêm dịch vụ thành công!", serviceId = service.ServiceID });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] MedicalServiceUpdateRequest request)
    {
        var service = _context.MedicalServices.Find(id);
        if (service == null)
            return NotFound();

        if (request.ServiceName != null)
            service.ServiceName = request.ServiceName;
        if (request.Description != null)
            service.Description = request.Description;
        if (request.Price.HasValue)
            service.Price = request.Price;
        if (request.DepartmentID.HasValue)
            service.DepartmentID = request.DepartmentID;
        if (request.Status != null)
            service.Status = request.Status;

        _context.SaveChanges();
        return Ok(new { message = "Cập nhật thành công!" });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var service = _context.MedicalServices.Find(id);
        if (service == null)
            return NotFound();

        _context.MedicalServices.Remove(service);
        _context.SaveChanges();

        return Ok(new { message = "Xóa dịch vụ thành công!" });
    }
}

public class MedicalServiceCreateRequest
{
    public string ServiceName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? DepartmentID { get; set; }
}

public class MedicalServiceUpdateRequest
{
    public string? ServiceName { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? DepartmentID { get; set; }
    public string? Status { get; set; }
}
