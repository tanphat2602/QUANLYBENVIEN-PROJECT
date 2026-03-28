using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class PrescriptionDetail
{
    [Key]
    public int DetailID { get; set; }
    public int? PrescriptionID { get; set; }
    public int? MedicineID { get; set; }
    public string? Dosage { get; set; }
    public string? Quantity { get; set; }
    public string? UsageInstructions { get; set; }
    public int? Duration { get; set; }

    // Navigation properties
    public virtual Prescription? Prescription { get; set; }
    public virtual Medicine? Medicine { get; set; }
}
