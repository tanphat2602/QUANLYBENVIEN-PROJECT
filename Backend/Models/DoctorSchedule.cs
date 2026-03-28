using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class DoctorSchedule
{
    [Key]
    public int ScheduleID { get; set; }
    public int? DoctorID { get; set; }
    public string? DayOfWeek { get; set; }
    public string? Shift { get; set; }
    public string? TimeStart { get; set; }
    public string? TimeEnd { get; set; }
    public int? MaxPatients { get; set; }
    public int? CurrentPatients { get; set; }
    public string? Status { get; set; }

    // Navigation properties
    public virtual Doctor? Doctor { get; set; }
}
