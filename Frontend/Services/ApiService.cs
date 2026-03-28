using System.Net.Http;
using System.Net.Http.Json;
using QuanLyBenhVien.Frontend.Models;

namespace QuanLyBenhVien.Frontend.Services;

public class ApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _apiBase = "http://localhost:5000/api";

    public ApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    private HttpClient GetClient()
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var token = _httpContextAccessor.HttpContext?.Session.GetString("Token");
        client.DefaultRequestHeaders.Authorization = null;
        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        return client;
    }

    // ==================== AUTH ====================
    public async Task<HttpResponseMessage> LoginAsync(string username, string password)
    {
        var content = JsonContent.Create(new { username, password });
        return await GetClient().PostAsync($"{_apiBase}/auth/login", content);
    }

    public async Task<HttpResponseMessage> RegisterAsync(RegisterViewModel model)
    {
        var content = JsonContent.Create(model);
        return await GetClient().PostAsync($"{_apiBase}/auth/register", content);
    }

    // ==================== DASHBOARD ====================
    public async Task<DashboardStats?> GetDashboardStatsAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/dashboard/stats");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<DashboardStats>();
        }
        return null;
    }

    // ==================== USERS ====================
    public async Task<List<User>> GetUsersAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/users");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<User>>() ?? new List<User>();
        }
        return new List<User>();
    }

    // ==================== PATIENTS ====================
    public async Task<List<Patient>> GetPatientsAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/patients");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Patient>>() ?? new List<Patient>();
        }
        return new List<Patient>();
    }

    public async Task<Patient?> GetPatientByIdAsync(int id)
    {
        var response = await GetClient().GetAsync($"{_apiBase}/patients/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Patient>();
        }
        return null;
    }

    public async Task<bool> CreatePatientAsync(PatientFormModel model)
    {
        var content = JsonContent.Create(model);
        var response = await GetClient().PostAsync($"{_apiBase}/patients", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdatePatientAsync(int id, PatientFormModel model)
    {
        var content = JsonContent.Create(model);
        var response = await GetClient().PutAsync($"{_apiBase}/patients/{id}", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeletePatientAsync(int id)
    {
        var response = await GetClient().DeleteAsync($"{_apiBase}/patients/{id}");
        return response.IsSuccessStatusCode;
    }

    // ==================== DOCTORS ====================
    public async Task<List<Doctor>> GetDoctorsAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/doctors");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Doctor>>() ?? new List<Doctor>();
        }
        return new List<Doctor>();
    }

    public async Task<bool> CreateDoctorAsync(DoctorFormModel model)
    {
        var content = JsonContent.Create(model);
        var response = await GetClient().PostAsync($"{_apiBase}/doctors", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateDoctorAsync(int id, DoctorFormModel model)
    {
        var content = JsonContent.Create(model);
        var response = await GetClient().PutAsync($"{_apiBase}/doctors/{id}", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteDoctorAsync(int id)
    {
        var response = await GetClient().DeleteAsync($"{_apiBase}/doctors/{id}");
        return response.IsSuccessStatusCode;
    }

    // ==================== DEPARTMENTS ====================
    public async Task<List<Department>> GetDepartmentsAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/departments");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Department>>() ?? new List<Department>();
        }
        return new List<Department>();
    }

    // ==================== APPOINTMENTS ====================
    public async Task<List<Appointment>> GetAppointmentsAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/appointments");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Appointment>>() ?? new List<Appointment>();
        }
        return new List<Appointment>();
    }

    public async Task<bool> CreateAppointmentAsync(AppointmentFormModel model)
    {
        var content = JsonContent.Create(model);
        var response = await GetClient().PostAsync($"{_apiBase}/appointments", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CancelAppointmentAsync(int id)
    {
        var response = await GetClient().DeleteAsync($"{_apiBase}/appointments/{id}");
        return response.IsSuccessStatusCode;
    }

    // ==================== ROOMS ====================
    public async Task<List<Room>> GetRoomsAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/rooms");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Room>>() ?? new List<Room>();
        }
        return new List<Room>();
    }

    // ==================== MEDICINES ====================
    public async Task<List<Medicine>> GetMedicinesAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/medicines");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Medicine>>() ?? new List<Medicine>();
        }
        return new List<Medicine>();
    }

    // ==================== PAYMENTS ====================
    public async Task<List<Payment>> GetPaymentsAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/payments");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Payment>>() ?? new List<Payment>();
        }
        return new List<Payment>();
    }

    // ==================== SERVICES ====================
    public async Task<List<MedicalService>> GetServicesAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/services");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<MedicalService>>() ?? new List<MedicalService>();
        }
        return new List<MedicalService>();
    }

    // ==================== MEDICAL RECORDS ====================
    public async Task<List<MedicalRecord>> GetMedicalRecordsAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/medicalrecords");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<MedicalRecord>>() ?? new List<MedicalRecord>();
        }
        return new List<MedicalRecord>();
    }

    // ==================== SCHEDULES ====================
    public async Task<List<DoctorSchedule>> GetSchedulesAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/schedules");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<DoctorSchedule>>() ?? new List<DoctorSchedule>();
        }
        return new List<DoctorSchedule>();
    }

    // ==================== EMERGENCY ====================
    public async Task<List<EmergencyRequest>> GetEmergenciesAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/emergency");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<EmergencyRequest>>() ?? new List<EmergencyRequest>();
        }
        return new List<EmergencyRequest>();
    }

    public async Task<bool> ProcessEmergencyAsync(int id, int doctorId)
    {
        var content = JsonContent.Create(new { emergencyId = id, doctorId });
        var response = await GetClient().PostAsync($"{_apiBase}/emergency/process", content);
        return response.IsSuccessStatusCode;
    }

    // ==================== NOTIFICATIONS ====================
    public async Task<List<Notification>> GetNotificationsAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/notifications");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Notification>>() ?? new List<Notification>();
        }
        return new List<Notification>();
    }

    // ==================== PATIENT PORTAL ====================
    public async Task<PatientProfile?> GetPatientProfileAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/patient/profile");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<PatientProfile>();
        }
        return null;
    }

    public async Task<bool> UpdatePatientProfileAsync(object updates)
    {
        var content = JsonContent.Create(updates);
        var response = await GetClient().PutAsync($"{_apiBase}/patient/profile", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<Appointment>> GetMyAppointmentsAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/patient/appointments");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Appointment>>() ?? new List<Appointment>();
        }
        return new List<Appointment>();
    }

    public async Task<bool> BookAppointmentAsync(BookAppointmentModel model)
    {
        var content = JsonContent.Create(model);
        var response = await GetClient().PostAsync($"{_apiBase}/patient/appointments", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CancelMyAppointmentAsync(int id)
    {
        var response = await GetClient().DeleteAsync($"{_apiBase}/patient/appointments/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<MedicalRecord>> GetMyMedicalRecordsAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/patient/medical-records");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<MedicalRecord>>() ?? new List<MedicalRecord>();
        }
        return new List<MedicalRecord>();
    }

    public async Task<List<Prescription>> GetMyPrescriptionsAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/patient/prescriptions");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Prescription>>() ?? new List<Prescription>();
        }
        return new List<Prescription>();
    }

    public async Task<List<string>> GetAvailableTimeSlotsAsync(int doctorId, string date)
    {
        var response = await GetClient().GetAsync($"{_apiBase}/patient/time-slots?doctorId={doctorId}&date={date}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<string>>() ?? new List<string>();
        }
        return new List<string>();
    }

    // ==================== DOCTOR PORTAL ====================
    public async Task<DoctorProfile?> GetDoctorProfileAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/doctor/profile");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<DoctorProfile>();
        }
        return null;
    }

    public async Task<List<Appointment>> GetDoctorScheduleAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/doctor/schedule");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Appointment>>() ?? new List<Appointment>();
        }
        return new List<Appointment>();
    }

    public async Task<object?> GetPatientDetailsAsync(int patientId)
    {
        var response = await GetClient().GetAsync($"{_apiBase}/doctor/patient/{patientId}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<object>();
        }
        return null;
    }

    public async Task<bool> CreateMedicalRecordAsync(CreateMedicalRecordModel model)
    {
        var content = JsonContent.Create(model);
        var response = await GetClient().PostAsync($"{_apiBase}/doctor/medical-record", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CreatePrescriptionAsync(CreatePrescriptionModel model)
    {
        var content = JsonContent.Create(model);
        var response = await GetClient().PostAsync($"{_apiBase}/doctor/prescription", content);
        return response.IsSuccessStatusCode;
    }

    // ==================== RECEPTIONIST PORTAL ====================
    public async Task<List<QueueItem>> GetTodayQueueAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/receptionist/queue");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<QueueItem>>() ?? new List<QueueItem>();
        }
        return new List<QueueItem>();
    }

    public async Task<bool> CheckInPatientAsync(int appointmentId)
    {
        var content = JsonContent.Create(new { appointmentId });
        var response = await GetClient().PostAsync($"{_apiBase}/receptionist/checkin", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CallPatientAsync(int appointmentId)
    {
        var response = await GetClient().PutAsync($"{_apiBase}/receptionist/appointment/{appointmentId}/call", null);
        return response.IsSuccessStatusCode;
    }

    // ==================== PHARMACIST PORTAL ====================
    public async Task<List<Medicine>> GetMedicinesWithStockAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/pharmacist/medicines");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Medicine>>() ?? new List<Medicine>();
        }
        return new List<Medicine>();
    }

    public async Task<bool> AddMedicineAsync(object medicine)
    {
        var content = JsonContent.Create(medicine);
        var response = await GetClient().PostAsync($"{_apiBase}/pharmacist/medicines", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<object>> GetPendingPrescriptionsAsync()
    {
        var response = await GetClient().GetAsync($"{_apiBase}/pharmacist/prescriptions/pending");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<object>>() ?? new List<object>();
        }
        return new List<object>();
    }

    public async Task<bool> DispensePrescriptionAsync(int prescriptionId)
    {
        var response = await GetClient().PutAsync($"{_apiBase}/pharmacist/prescriptions/{prescriptionId}/dispense", null);
        return response.IsSuccessStatusCode;
    }
}

// Form Models
public class PatientFormModel
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? InsuranceProvider { get; set; }
    public string? PolicyNumber { get; set; }
}

public class DoctorFormModel
{
    public int? UserID { get; set; }
    public int? DepartmentID { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Specialty { get; set; }
    public string? Experience { get; set; }
    public decimal? ConsultationFee { get; set; }
    public string? WorkingSchedule { get; set; }
}

public class AppointmentFormModel
{
    public int? PatientID { get; set; }
    public int? DoctorID { get; set; }
    public int? DepartmentID { get; set; }
    public DateTime? AppointmentDate { get; set; }
    public string? TimeSlot { get; set; }
    public string? Reason { get; set; }
    public string? AppointmentType { get; set; }
    public bool IsEmergency { get; set; }
}
