using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBenhVien.API.Data;
using QuanLyBenhVien.API.Models;

namespace QuanLyBenhVien.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RoomsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var rooms = _context.Rooms
            .Include(r => r.Department)
            .ToList()
            .Select(r => new
            {
                r.RoomID,
                r.RoomNumber,
                r.DepartmentID,
                DepartmentName = r.Department?.DepartmentName,
                r.RoomType,
                r.Status
            });

        return Ok(rooms);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var room = _context.Rooms
            .Include(r => r.Department)
            .FirstOrDefault(r => r.RoomID == id);

        if (room == null)
            return NotFound();

        return Ok(new
        {
            room.RoomID,
            room.RoomNumber,
            room.DepartmentID,
            DepartmentName = room.Department?.DepartmentName,
            room.RoomType,
            room.Status
        });
    }

    [HttpGet("department/{departmentId}")]
    public IActionResult GetByDepartment(int departmentId)
    {
        var rooms = _context.Rooms
            .Where(r => r.DepartmentID == departmentId)
            .ToList()
            .Select(r => new
            {
                r.RoomID,
                r.RoomNumber,
                r.RoomType,
                r.Status
            });

        return Ok(rooms);
    }

    [HttpPost]
    public IActionResult Create([FromBody] RoomCreateRequest request)
    {
        var room = new Room
        {
            RoomNumber = request.RoomNumber,
            DepartmentID = request.DepartmentID,
            RoomType = request.RoomType,
            Status = "Hoạt động"
        };

        _context.Rooms.Add(room);
        _context.SaveChanges();

        return Ok(new { message = "Thêm phòng thành công!", roomId = room.RoomID });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] RoomUpdateRequest request)
    {
        var room = _context.Rooms.Find(id);
        if (room == null)
            return NotFound();

        if (request.RoomNumber != null)
            room.RoomNumber = request.RoomNumber;
        if (request.DepartmentID.HasValue)
            room.DepartmentID = request.DepartmentID;
        if (request.RoomType != null)
            room.RoomType = request.RoomType;
        if (request.Status != null)
            room.Status = request.Status;

        _context.SaveChanges();
        return Ok(new { message = "Cập nhật thành công!" });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var room = _context.Rooms.Find(id);
        if (room == null)
            return NotFound();

        _context.Rooms.Remove(room);
        _context.SaveChanges();

        return Ok(new { message = "Xóa phòng thành công!" });
    }
}

public class RoomCreateRequest
{
    public string RoomNumber { get; set; } = string.Empty;
    public int? DepartmentID { get; set; }
    public string? RoomType { get; set; }
}

public class RoomUpdateRequest
{
    public string? RoomNumber { get; set; }
    public int? DepartmentID { get; set; }
    public string? RoomType { get; set; }
    public string? Status { get; set; }
}
