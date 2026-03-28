using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class Notification
{
    [Key]
    public int NotificationID { get; set; }
    public int? UserID { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Type { get; set; }
    public bool? IsRead { get; set; }
    public DateTime? CreatedAt { get; set; }

    // Navigation properties
    public virtual User? User { get; set; }
}
