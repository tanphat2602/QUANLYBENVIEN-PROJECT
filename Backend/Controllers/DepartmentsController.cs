using Microsoft.AspNetCore.Mvc;
using QuanLyBenhVien.API.Data;

namespace QuanLyBenhVien.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DepartmentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var departments = _context.Departments.ToList();
        return Ok(departments);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var department = _context.Departments.Find(id);
        if (department == null)
            return NotFound();
        return Ok(department);
    }
}
