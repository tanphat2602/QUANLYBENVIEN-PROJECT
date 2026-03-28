using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class Patient
{
    [Key]
    public int PatientID { get; set; }
    public int? UserID { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Status { get; set; }
    public string? BloodType { get; set; }
    public string? Allergies { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    public int? InsuranceID { get; set; }

    // Navigation properties
    public virtual User? User { get; set; }
    public virtual Insurance? Insurance { get; set; }
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
