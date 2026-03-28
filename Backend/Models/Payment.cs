using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class Payment
{
    [Key]
    public int PaymentID { get; set; }
    public int? PatientID { get; set; }
    public int? AppointmentID { get; set; }
    public decimal? Amount { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Status { get; set; }
    public string? TransactionID { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string? Description { get; set; }

    // Navigation properties
    public virtual Patient? Patient { get; set; }
    public virtual Appointment? Appointment { get; set; }
}
