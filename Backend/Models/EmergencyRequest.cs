using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class EmergencyRequest
{
    [Key]
    public int EmergencyID { get; set; }
    public int? PatientID { get; set; }
    public int? AssignedDoctorID { get; set; }
    public string? Reason { get; set; }
    public string? Symptoms { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public DateTime? RequestedAt { get; set; }
    public DateTime? AssignedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Navigation properties
    public virtual Patient? Patient { get; set; }
    public virtual Doctor? AssignedDoctor { get; set; }
}
