using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class Medicine
{
    [Key]
    public int MedicineID { get; set; }
    public string MedicineName { get; set; } = string.Empty;
    public string? GenericName { get; set; }
    public string? Manufacturer { get; set; }
    public string? Unit { get; set; }
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
    public string? DosageForm { get; set; }
    public string? SideEffects { get; set; }
    public string? Contraindications { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Status { get; set; }

    // Navigation properties
    public virtual ICollection<PrescriptionDetail> PrescriptionDetails { get; set; } = new List<PrescriptionDetail>();
}
