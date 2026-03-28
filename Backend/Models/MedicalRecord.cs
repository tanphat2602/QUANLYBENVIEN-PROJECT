using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class MedicalRecord
{
    [Key]
    public int RecordID { get; set; }
    public int? PatientID { get; set; }
    public int? DoctorID { get; set; }
    public int? AppointmentID { get; set; }
    public string? Symptoms { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Notes { get; set; }
    public string? Status { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? VitalSigns { get; set; }

    // Navigation properties
    public virtual Patient? Patient { get; set; }
    public virtual Doctor? Doctor { get; set; }
    public virtual Appointment? Appointment { get; set; }
    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    public virtual ICollection<TestRequest> TestRequests { get; set; } = new List<TestRequest>();
}
