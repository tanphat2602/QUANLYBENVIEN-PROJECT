namespace QuanLyBenhVien.Frontend.Models;

public class User
{
    public int UserID { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Status { get; set; }
    public string? Address { get; set; }
    public List<string>? Roles { get; set; }
    public List<string>? Permissions { get; set; }
}

public class Patient
{
    public int PatientID { get; set; }
    public int? UserID { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? InsuranceProvider { get; set; }
    public string? PolicyNumber { get; set; }
    public string? BloodType { get; set; }
    public double? Height { get; set; }
    public double? Weight { get; set; }
    public string? Allergies { get; set; }
    public string? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class Doctor
{
    public int DoctorID { get; set; }
    public int? UserID { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Specialty { get; set; }
    public string? WorkingSchedule { get; set; }
    public string? DepartmentName { get; set; }
    public int? DepartmentID { get; set; }
    public string? Experience { get; set; }
    public string? Education { get; set; }
    public decimal? ConsultationFee { get; set; }
    public string? Status { get; set; }
}

public class Appointment
{
    public int AppointmentID { get; set; }
    public int? PatientID { get; set; }
    public int? DoctorID { get; set; }
    public int? DepartmentID { get; set; }
    public string? PatientName { get; set; }
    public string? DoctorName { get; set; }
    public string? DepartmentName { get; set; }
    public string? AppointmentDate { get; set; }
    public string? TimeSlot { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
    public string? Reason { get; set; }
    public string? AppointmentType { get; set; }
    public bool IsEmergency { get; set; }
    public int Priority { get; set; }
    public string? CreatedAt { get; set; }
}

public class Department
{
    public int DepartmentID { get; set; }
    public string? DepartmentName { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? Status { get; set; }
}

public class Room
{
    public int RoomID { get; set; }
    public string? RoomNumber { get; set; }
    public int? DepartmentID { get; set; }
    public string? DepartmentName { get; set; }
    public string? RoomType { get; set; }
    public string? Status { get; set; }
}

public class Medicine
{
    public int MedicineID { get; set; }
    public string? MedicineName { get; set; }
    public string? GenericName { get; set; }
    public string? Manufacturer { get; set; }
    public string? Unit { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? DosageForm { get; set; }
    public string? Status { get; set; }
}

public class Payment
{
    public int PaymentID { get; set; }
    public int? PatientID { get; set; }
    public int? AppointmentID { get; set; }
    public string? PatientName { get; set; }
    public decimal Amount { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Status { get; set; }
    public string? PaymentDate { get; set; }
    public string? CreatedAt { get; set; }
}

public class MedicalService
{
    public int ServiceID { get; set; }
    public string? ServiceName { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int? DepartmentID { get; set; }
    public string? Status { get; set; }
}

public class MedicalRecord
{
    public int RecordID { get; set; }
    public int? PatientID { get; set; }
    public int? DoctorID { get; set; }
    public int? AppointmentID { get; set; }
    public string? PatientName { get; set; }
    public string? DoctorName { get; set; }
    public string? Symptoms { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Notes { get; set; }
    public string? VitalSigns { get; set; }
    public string? Status { get; set; }
    public string? DateCreated { get; set; }
}

public class Prescription
{
    public int PrescriptionID { get; set; }
    public int? RecordID { get; set; }
    public string? Diagnosis { get; set; }
    public string? Status { get; set; }
    public string? CreatedAt { get; set; }
    public List<PrescriptionDetail>? Details { get; set; }
}

public class PrescriptionDetail
{
    public int DetailID { get; set; }
    public int? PrescriptionID { get; set; }
    public int? MedicineID { get; set; }
    public string? MedicineName { get; set; }
    public string? Dosage { get; set; }
    public int Quantity { get; set; }
    public string? Instructions { get; set; }
    public int? Stock { get; set; }
}

public class DoctorSchedule
{
    public int ScheduleID { get; set; }
    public int? DoctorID { get; set; }
    public string? DoctorName { get; set; }
    public string? DayOfWeek { get; set; }
    public string? Shift { get; set; }
    public string? TimeStart { get; set; }
    public string? TimeEnd { get; set; }
    public int MaxPatients { get; set; }
    public int CurrentPatients { get; set; }
    public string? Status { get; set; }
}

public class Notification
{
    public int NotificationID { get; set; }
    public int? UserID { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
    public string? Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class EmergencyRequest
{
    public int RequestID { get; set; }
    public int? PatientID { get; set; }
    public int? AssignedDoctorID { get; set; }
    public string? PatientName { get; set; }
    public string? AssignedDoctorName { get; set; }
    public string? Reason { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public string? EmergencyType { get; set; }
    public DateTime? RequestTime { get; set; }
    public DateTime? ProcessedTime { get; set; }
}

public class QueueItem
{
    public int AppointmentID { get; set; }
    public string? AppointmentDate { get; set; }
    public string? TimeSlot { get; set; }
    public string? Status { get; set; }
    public string? Reason { get; set; }
    public bool IsEmergency { get; set; }
    public int PatientID { get; set; }
    public string? PatientName { get; set; }
    public string? PatientPhone { get; set; }
    public int DoctorID { get; set; }
    public string? DoctorName { get; set; }
    public string? Specialty { get; set; }
}

public class DashboardStats
{
    public int TotalUsers { get; set; }
    public int TotalDoctors { get; set; }
    public int TotalPatients { get; set; }
    public int TotalAppointments { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalDepartments { get; set; }
    public int PendingAppointments { get; set; }
    public int TodayAppointments { get; set; }
    public int EmergencyRequests { get; set; }
}

public class LoginViewModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterViewModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Gender { get; set; }
}

// Patient Portal ViewModels
public class PatientProfile
{
    public int PatientID { get; set; }
    public int UserID { get; set; }
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? DateOfBirth { get; set; }
    public string? BloodType { get; set; }
    public double? Height { get; set; }
    public double? Weight { get; set; }
    public string? Allergies { get; set; }
    public int? InsuranceID { get; set; }
    public string? Status { get; set; }
}

public class BookAppointmentModel
{
    public int DoctorID { get; set; }
    public int DepartmentID { get; set; }
    public string AppointmentDate { get; set; } = string.Empty;
    public string TimeSlot { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string AppointmentType { get; set; } = "Khám bệnh";
    public bool IsEmergency { get; set; }
}

// Doctor Portal ViewModels
public class DoctorProfile
{
    public int DoctorID { get; set; }
    public int UserID { get; set; }
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Gender { get; set; }
    public string? Specialty { get; set; }
    public string? Experience { get; set; }
    public string? Education { get; set; }
    public decimal? ConsultationFee { get; set; }
    public string? WorkingSchedule { get; set; }
    public string? Status { get; set; }
}

public class CreateMedicalRecordModel
{
    public int PatientID { get; set; }
    public int? AppointmentID { get; set; }
    public string Symptoms { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? VitalSigns { get; set; }
}

public class CreatePrescriptionModel
{
    public int RecordID { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public List<PrescriptionMedicineModel> Medicines { get; set; } = new();
}

public class PrescriptionMedicineModel
{
    public int MedicineID { get; set; }
    public string Dosage { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Instructions { get; set; } = string.Empty;
}

// Pharmacist ViewModels
public class InventoryReport
{
    public int TotalMedicines { get; set; }
    public decimal TotalValue { get; set; }
    public int LowStockCount { get; set; }
    public int OutOfStockCount { get; set; }
    public List<Medicine> Medicines { get; set; } = new();
}
