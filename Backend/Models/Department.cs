using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class Department
{
    [Key]
    public int DepartmentID { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? Status { get; set; }

    // Navigation properties
    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
