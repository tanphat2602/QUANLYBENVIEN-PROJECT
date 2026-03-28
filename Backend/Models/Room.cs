using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class Room
{
    [Key]
    public int RoomID { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public int? DepartmentID { get; set; }
    public string? RoomType { get; set; }
    public string? Status { get; set; }

    // Navigation properties
    public virtual Department? Department { get; set; }
    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
