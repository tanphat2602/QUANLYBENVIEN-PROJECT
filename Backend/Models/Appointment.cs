using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class Appointment
{
    [Key]
    public int AppointmentID { get; set; }
    public int? PatientID { get; set; }
    public int? DoctorID { get; set; }
    public int? DepartmentID { get; set; }
    public DateTime? AppointmentDate { get; set; }
    public string? TimeSlot { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
    public string? Reason { get; set; }
    public string? AppointmentType { get; set; }
    public bool? IsEmergency { get; set; }
    public int? Priority { get; set; }
    public DateTime? CreatedAt { get; set; }

    // Navigation properties
    public virtual Patient? Patient { get; set; }
    public virtual Doctor? Doctor { get; set; }
    public virtual Department? Department { get; set; }
}
