using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBenhVien.API.Data;
using QuanLyBenhVien.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace QuanLyBenhVien.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefault(u => u.Username == request.Username);

        if (user == null)
        {
            return BadRequest(new { message = "Tài khoản không tồn tại!" });
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return BadRequest(new { message = "Mật khẩu không đúng!" });
        }

        if (user.Status != "Hoạt động")
        {
            return BadRequest(new { message = "Tài khoản đã bị khóa!" });
        }

        var roleName = user.UserRoles.FirstOrDefault()?.Role?.RoleName ?? "Patient";

        var token = GenerateJwtToken(user.UserID.ToString(), user.Username, roleName);

        return Ok(new
        {
            token,
            userId = user.UserID,
            username = user.Username,
            fullName = user.FullName,
            role = roleName,
            email = user.Email
        });
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (_context.Users.Any(u => u.Email == request.Email))
        {
            return BadRequest(new { message = "Email này đã được sử dụng!" });
        }

        if (request.Password.Length < 8)
        {
            return BadRequest(new { message = "Mật khẩu phải có ít nhất 8 ký tự!" });
        }

        var newUser = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Email = request.Email,
            FullName = request.FullName,
            Phone = request.Phone,
            Gender = request.Gender,
            Status = "Hoạt động",
            CreatedAt = DateTime.Now
        };

        _context.Users.Add(newUser);
        _context.SaveChanges();

        var patientRole = _context.Roles.FirstOrDefault(r => r.RoleName == "Patient");
        if (patientRole != null)
        {
            _context.UserRoles.Add(new UserRole
            {
                UserID = newUser.UserID,
                RoleID = patientRole.RoleID
            });
            _context.SaveChanges();
        }

        var patient = new Patient
        {
            UserID = newUser.UserID
        };
        _context.Patients.Add(patient);
        _context.SaveChanges();

        return Ok(new { message = "Đăng ký thành công!" });
    }

    private string GenerateJwtToken(string userId, string username, string role)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("QuanLyBenhVienSecretKey2024!@#$%^&*()_+VeryLongKey"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Gender { get; set; }
}
