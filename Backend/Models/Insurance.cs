using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class Insurance
{
    [Key]
    public int InsuranceID { get; set; }
    public string? InsuranceProvider { get; set; }
    public string? PolicyNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public decimal? CoverageAmount { get; set; }

    // Navigation properties
    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
}
