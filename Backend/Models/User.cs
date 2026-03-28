using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class User
{
    [Key]
    public int UserID { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? FullName { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Status { get; set; }
    public string? Address { get; set; }
    public string? CCCD { get; set; }
    public string? Avatar { get; set; }

    // Navigation properties
    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
}
