using Microsoft.AspNetCore.Mvc;
using QuanLyBenhVien.API.Data;
using QuanLyBenhVien.API.Models;

namespace QuanLyBenhVien.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public NotificationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var notifications = _context.Notifications
            .OrderByDescending(n => n.CreatedAt)
            .ToList();
        return Ok(notifications);
    }

    [HttpGet("user/{userId}")]
    public IActionResult GetByUser(int userId)
    {
        var notifications = _context.Notifications
            .Where(n => n.UserID == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToList();
        return Ok(notifications);
    }

    [HttpGet("unread/{userId}")]
    public IActionResult GetUnread(int userId)
    {
        var count = _context.Notifications
            .Count(n => n.UserID == userId && n.IsRead == false);
        return Ok(new { count });
    }

    [HttpPut("{id}/read")]
    public IActionResult MarkAsRead(int id)
    {
        var notification = _context.Notifications.Find(id);
        if (notification == null)
            return NotFound();

        notification.IsRead = true;
        _context.SaveChanges();

        return Ok(new { message = "Đã đánh dấu đã đọc!" });
    }

    [HttpPost]
    public IActionResult Create([FromBody] NotificationCreateRequest request)
    {
        var notification = new Notification
        {
            UserID = request.UserID,
            Title = request.Title,
            Content = request.Content,
            Type = request.Type,
            IsRead = false,
            CreatedAt = DateTime.Now
        };

        _context.Notifications.Add(notification);
        _context.SaveChanges();

        return Ok(new { message = "Gửi thông báo thành công!" });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var notification = _context.Notifications.Find(id);
        if (notification == null)
            return NotFound();

        _context.Notifications.Remove(notification);
        _context.SaveChanges();

        return Ok(new { message = "Xóa thông báo thành công!" });
    }
}

public class NotificationCreateRequest
{
    public int? UserID { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Type { get; set; }
}
