using Microsoft.AspNetCore.Mvc;
using QuanLyBenhVien.Frontend.Models;
using QuanLyBenhVien.Frontend.Services;

namespace QuanLyBenhVien.Frontend.Controllers;

public class AdminController : Controller
{
    private readonly ApiService _apiService;

    public AdminController(ApiService apiService)
    {
        _apiService = apiService;
    }

    private bool IsAuthenticated()
    {
        return !string.IsNullOrEmpty(HttpContext.Session.GetString("Token"));
    }

    private bool IsAdmin()
    {
        return HttpContext.Session.GetString("Role") == "Admin";
    }

    private IActionResult RedirectToLogin()
    {
        return RedirectToAction("Login", "Account", new { area = "" });
    }

    // ==================== DASHBOARD ====================
    public IActionResult Dashboard()
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var stats = _apiService.GetDashboardStatsAsync().Result;
        ViewBag.Stats = stats;
        ViewBag.FullName = HttpContext.Session.GetString("FullName");
        ViewBag.Role = HttpContext.Session.GetString("Role");
        return View();
    }

    // ==================== PATIENTS ====================
    public IActionResult Patients()
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var patients = _apiService.GetPatientsAsync().Result;
        ViewBag.Title = "Quản lý Bệnh nhân";
        ViewBag.Controller = "Patients";
        return View("~/Views/Admin/List.cshtml", patients);
    }

    [HttpGet]
    public IActionResult CreatePatient()
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        ViewBag.Title = "Thêm Bệnh nhân";
        ViewBag.Action = "CreatePatient";
        ViewBag.Controller = "Patients";
        return View("~/Views/Admin/Form.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> CreatePatient([FromForm] PatientFormModel model)
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var result = await _apiService.CreatePatientAsync(model);
        if (result)
        {
            TempData["Success"] = "Thêm bệnh nhân thành công!";
            return RedirectToAction("Patients");
        }
        ViewBag.Error = "Có lỗi xảy ra. Vui lòng thử lại.";
        return View("~/Views/Admin/Form.cshtml", model);
    }

    [HttpGet]
    public IActionResult EditPatient(int id)
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var patients = _apiService.GetPatientsAsync().Result;
        var patient = patients?.FirstOrDefault(p => p.PatientID == id);
        
        if (patient == null)
        {
            return NotFound();
        }
        
        ViewBag.Title = "Sửa Bệnh nhân";
        ViewBag.Action = $"EditPatient/{id}";
        ViewBag.Controller = "Patients";
        return View("~/Views/Admin/Form.cshtml", patient);
    }

    [HttpPost]
    public async Task<IActionResult> EditPatient(int id, [FromForm] PatientFormModel model)
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var result = await _apiService.UpdatePatientAsync(id, model);
        if (result)
        {
            TempData["Success"] = "Cập nhật bệnh nhân thành công!";
            return RedirectToAction("Patients");
        }
        ViewBag.Error = "Có lỗi xảy ra. Vui lòng thử lại.";
        return View("~/Views/Admin/Form.cshtml", model);
    }

    public async Task<IActionResult> DeletePatient(int id)
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var result = await _apiService.DeletePatientAsync(id);
        TempData[result ? "Success" : "Error"] = result ? "Xóa thành công!" : "Có lỗi xảy ra.";
        return RedirectToAction("Patients");
    }

    // ==================== DOCTORS ====================
    public IActionResult Doctors()
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var doctors = _apiService.GetDoctorsAsync().Result;
        ViewBag.Title = "Quản lý Bác sĩ";
        ViewBag.Controller = "Doctors";
        return View("~/Views/Admin/List.cshtml", doctors);
    }

    [HttpGet]
    public IActionResult CreateDoctor()
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        ViewBag.Title = "Thêm Bác sĩ";
        ViewBag.Action = "CreateDoctor";
        ViewBag.Controller = "Doctors";
        ViewBag.Departments = _apiService.GetDepartmentsAsync().Result;
        return View("~/Views/Admin/DoctorForm.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> CreateDoctor([FromForm] DoctorFormModel model)
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var result = await _apiService.CreateDoctorAsync(model);
        if (result)
        {
            TempData["Success"] = "Thêm bác sĩ thành công!";
            return RedirectToAction("Doctors");
        }
        ViewBag.Error = "Có lỗi xảy ra. Vui lòng thử lại.";
        ViewBag.Departments = _apiService.GetDepartmentsAsync().Result;
        return View("~/Views/Admin/DoctorForm.cshtml", model);
    }

    [HttpGet]
    public IActionResult EditDoctor(int id)
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var doctors = _apiService.GetDoctorsAsync().Result;
        var doctor = doctors?.FirstOrDefault(d => d.DoctorID == id);
        
        if (doctor == null) return NotFound();
        
        ViewBag.Title = "Sửa Bác sĩ";
        ViewBag.Action = $"EditDoctor/{id}";
        ViewBag.Controller = "Doctors";
        ViewBag.Departments = _apiService.GetDepartmentsAsync().Result;
        return View("~/Views/Admin/DoctorForm.cshtml", doctor);
    }

    [HttpPost]
    public async Task<IActionResult> EditDoctor(int id, [FromForm] DoctorFormModel model)
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var result = await _apiService.UpdateDoctorAsync(id, model);
        if (result)
        {
            TempData["Success"] = "Cập nhật bác sĩ thành công!";
            return RedirectToAction("Doctors");
        }
        ViewBag.Error = "Có lỗi xảy ra.";
        ViewBag.Departments = _apiService.GetDepartmentsAsync().Result;
        return View("~/Views/Admin/DoctorForm.cshtml", model);
    }

    public async Task<IActionResult> DeleteDoctor(int id)
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var result = await _apiService.DeleteDoctorAsync(id);
        TempData[result ? "Success" : "Error"] = result ? "Xóa thành công!" : "Có lỗi xảy ra.";
        return RedirectToAction("Doctors");
    }

    // ==================== APPOINTMENTS ====================
    public IActionResult Appointments()
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var appointments = _apiService.GetAppointmentsAsync().Result;
        ViewBag.Title = "Quản lý Lịch hẹn";
        ViewBag.Controller = "Appointments";
        return View("~/Views/Admin/Appointments.cshtml", appointments);
    }

    [HttpGet]
    public IActionResult CreateAppointment()
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        ViewBag.Title = "Tạo Lịch hẹn";
        ViewBag.Action = "CreateAppointment";
        ViewBag.Controller = "Appointments";
        ViewBag.Patients = _apiService.GetPatientsAsync().Result;
        ViewBag.Doctors = _apiService.GetDoctorsAsync().Result;
        ViewBag.Departments = _apiService.GetDepartmentsAsync().Result;
        return View("~/Views/Admin/AppointmentForm.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment([FromForm] AppointmentFormModel model)
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var result = await _apiService.CreateAppointmentAsync(model);
        if (result)
        {
            TempData["Success"] = "Tạo lịch hẹn thành công!";
            return RedirectToAction("Appointments");
        }
        ViewBag.Error = "Có lỗi xảy ra.";
        ViewBag.Patients = _apiService.GetPatientsAsync().Result;
        ViewBag.Doctors = _apiService.GetDoctorsAsync().Result;
        ViewBag.Departments = _apiService.GetDepartmentsAsync().Result;
        return View("~/Views/Admin/AppointmentForm.cshtml", model);
    }

    public async Task<IActionResult> CancelAppointment(int id)
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var result = await _apiService.CancelAppointmentAsync(id);
        TempData[result ? "Success" : "Error"] = result ? "Hủy lịch hẹn thành công!" : "Có lỗi xảy ra.";
        return RedirectToAction("Appointments");
    }

    // ==================== DEPARTMENTS ====================
    public IActionResult Departments()
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var departments = _apiService.GetDepartmentsAsync().Result;
        ViewBag.Title = "Quản lý Khoa";
        ViewBag.Controller = "Departments";
        return View("~/Views/Admin/List.cshtml", departments);
    }

    // ==================== MEDICINES ====================
    public IActionResult Medicines()
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var medicines = _apiService.GetMedicinesAsync().Result;
        ViewBag.Title = "Quản lý Thuốc";
        ViewBag.Controller = "Medicines";
        return View("~/Views/Admin/Medicines.cshtml", medicines);
    }

    // ==================== EMERGENCY ====================
    public IActionResult Emergency()
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var emergencies = _apiService.GetEmergenciesAsync().Result;
        ViewBag.Title = "Cấp cứu";
        ViewBag.Controller = "Emergency";
        return View("~/Views/Admin/Emergency.cshtml", emergencies);
    }

    [HttpPost]
    public async Task<IActionResult> ProcessEmergency(int id, [FromForm] int doctorId)
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var result = await _apiService.ProcessEmergencyAsync(id, doctorId);
        TempData[result ? "Success" : "Error"] = result ? "Xử lý cấp cứu thành công!" : "Có lỗi xảy ra.";
        return RedirectToAction("Emergency");
    }

    // ==================== PAYMENTS ====================
    public IActionResult Payments()
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var payments = _apiService.GetPaymentsAsync().Result;
        ViewBag.Title = "Thanh toán";
        return View("~/Views/Admin/Payments.cshtml", payments);
    }

    // ==================== MEDICAL RECORDS ====================
    public IActionResult MedicalRecords()
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        var records = _apiService.GetMedicalRecordsAsync().Result;
        ViewBag.Title = "Hồ sơ Bệnh án";
        return View("~/Views/Admin/MedicalRecords.cshtml", records);
    }

    // ==================== REPORTS ====================
    public IActionResult Reports()
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        ViewBag.Title = "Báo cáo Thống kê";
        return View("~/Views/Admin/Reports.cshtml");
    }

    // ==================== USERS MANAGEMENT ====================
    public IActionResult Users()
    {
        if (!IsAdmin()) return RedirectToLogin();
        
        var users = _apiService.GetUsersAsync().Result;
        ViewBag.Title = "Quản lý Người dùng";
        ViewBag.Controller = "Users";
        return View("~/Views/Admin/Users.cshtml", users);
    }

    // ==================== SETTINGS ====================
    public IActionResult Settings()
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        
        ViewBag.Title = "Cài đặt";
        return View("~/Views/Admin/Settings.cshtml");
    }
}
