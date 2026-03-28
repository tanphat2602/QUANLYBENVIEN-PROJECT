using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class MedicalService
{
    [Key]
    public int ServiceID { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? DepartmentID { get; set; }
    public string? Status { get; set; }
}
