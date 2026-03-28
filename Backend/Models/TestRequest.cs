using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class TestRequest
{
    [Key]
    public int TestID { get; set; }
    public int? RecordID { get; set; }
    public int? PatientID { get; set; }
    public string? TestType { get; set; }
    public string? Description { get; set; }
    public DateTime? RequestDate { get; set; }
    public DateTime? ResultDate { get; set; }
    public string? Result { get; set; }
    public string? Status { get; set; }
    public decimal? Fee { get; set; }

    // Navigation properties
    public virtual MedicalRecord? MedicalRecord { get; set; }
    public virtual Patient? Patient { get; set; }
}
