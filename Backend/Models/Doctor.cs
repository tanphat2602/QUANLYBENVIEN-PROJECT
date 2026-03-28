using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class Doctor
{
    [Key]
    public int DoctorID { get; set; }
    public int? UserID { get; set; }
    public int? DepartmentID { get; set; }
    public string? Specialty { get; set; }
    public string? WorkingSchedule { get; set; }
    public string? Experience { get; set; }
    public string? Education { get; set; }
    public decimal? ConsultationFee { get; set; }
    public string? Status { get; set; }
    public int? RoomID { get; set; }

    // Navigation properties
    public virtual User? User { get; set; }
    public virtual Department? Department { get; set; }
    public virtual Room? Room { get; set; }
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    public virtual ICollection<DoctorSchedule> Schedules { get; set; } = new List<DoctorSchedule>();
}
