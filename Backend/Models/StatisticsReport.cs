using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class StatisticsReport
{
    [Key]
    public int ReportID { get; set; }
    public string? ReportType { get; set; }
    public DateTime? ReportDate { get; set; }
    public int? TotalPatients { get; set; }
    public int? TotalAppointments { get; set; }
    public decimal? TotalRevenue { get; set; }
    public string? Details { get; set; }
}
