using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class Prescription
{
    [Key]
    public int PrescriptionID { get; set; }
    public int? RecordID { get; set; }
    public string? Diagnosis { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Status { get; set; }

    // Navigation properties
    public virtual MedicalRecord? MedicalRecord { get; set; }
    public virtual ICollection<PrescriptionDetail> Details { get; set; } = new List<PrescriptionDetail>();
}
