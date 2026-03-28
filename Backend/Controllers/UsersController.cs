using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using QuanLyBenhVien.API.Data;
using QuanLyBenhVien.API.Models;

namespace QuanLyBenhVien.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ToList()
            .Select(u => new
            {
                u.UserID,
                u.Username,
                u.Email,
                u.FullName,
                u.Phone,
                u.Gender,
                u.DateOfBirth,
                u.Status,
                u.CreatedAt,
                Roles = u.UserRoles.Select(ur => ur.Role != null ? ur.Role.RoleName : "").ToList()
            })
            .ToList();

        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefault(u => u.UserID == id);

        if (user == null)
            return NotFound();

        return Ok(new
        {
            user.UserID,
            user.Username,
            user.Email,
            user.FullName,
            user.Phone,
            user.Gender,
            user.DateOfBirth,
            user.Status,
            user.CreatedAt,
            Roles = user.UserRoles.Select(ur => ur.Role != null ? ur.Role.RoleName : "").ToList()
        });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UserUpdateRequest request)
    {
        var user = _context.Users.Find(id);
        if (user == null)
            return NotFound();

        user.FullName = request.FullName ?? user.FullName;
        user.Email = request.Email ?? user.Email;
        user.Phone = request.Phone ?? user.Phone;
        user.Gender = request.Gender ?? user.Gender;
        user.DateOfBirth = request.DateOfBirth ?? user.DateOfBirth;
        user.Status = request.Status ?? user.Status;

        _context.SaveChanges();
        return Ok(new { message = "Cập nhật thành công!" });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
            return NotFound();

        _context.Users.Remove(user);
        _context.SaveChanges();

        return Ok(new { message = "Xóa thành công!" });
    }
}

public class UserUpdateRequest
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Status { get; set; }
}
