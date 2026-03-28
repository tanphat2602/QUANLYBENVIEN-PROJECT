using Microsoft.AspNetCore.Mvc;
using QuanLyBenhVien.Frontend.Services;
using QuanLyBenhVien.Frontend.Models;

namespace QuanLyBenhVien.Frontend.Controllers;

public class PatientController : Controller
{
    private readonly ApiService _apiService;

    public PatientController(ApiService apiService)
    {
        _apiService = apiService;
    }

    private bool IsPatient()
    {
        return HttpContext.Session.GetString("Role") == "PATIENT";
    }

    // GET /Patient - Patient Dashboard
    public async Task<IActionResult> Index()
    {
        if (!IsPatient())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        ViewBag.FullName = HttpContext.Session.GetString("FullName");
        ViewBag.Role = "PATIENT";

        // Get upcoming appointments
        var appointments = await _apiService.GetMyAppointmentsAsync();
        ViewBag.UpcomingAppointments = appointments?.Where(a => 
            !string.IsNullOrEmpty(a.AppointmentDate) && 
            DateTime.TryParse(a.AppointmentDate, out var dt) && 
            dt >= DateTime.Today &&
            a.Status != "Đã hủy").Take(5).ToList() ?? new List<Appointment>();

        return View();
    }

    // GET /Patient/Profile
    public async Task<IActionResult> Profile()
    {
        if (!IsPatient())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var profile = await _apiService.GetPatientProfileAsync();
        return View(profile);
    }

    // POST /Patient/Profile
    [HttpPost]
    public async Task<IActionResult> Profile(PatientProfile model)
    {
        if (!IsPatient())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var updates = new
        {
            fullName = model.FullName,
            phone = model.Phone,
            address = model.Address,
            bloodType = model.BloodType,
            height = model.Height,
            weight = model.Weight,
            allergies = model.Allergies
        };

        var success = await _apiService.UpdatePatientProfileAsync(updates);
        if (success)
        {
            TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            HttpContext.Session.SetString("FullName", model.FullName ?? "");
        }
        else
        {
            TempData["ErrorMessage"] = "Cập nhật thông tin thất bại!";
        }

        return RedirectToAction("Profile");
    }

    // GET /Patient/BookAppointment
    public async Task<IActionResult> BookAppointment()
    {
        if (!IsPatient())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var doctors = await _apiService.GetDoctorsAsync();
        var departments = await _apiService.GetDepartmentsAsync();
        ViewBag.Doctors = doctors ?? new List<Doctor>();
        ViewBag.Departments = departments ?? new List<Department>();
        return View();
    }

    // POST /Patient/BookAppointment
    [HttpPost]
    public async Task<IActionResult> BookAppointment(BookAppointmentModel model)
    {
        if (!IsPatient())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var success = await _apiService.BookAppointmentAsync(model);
        if (success)
        {
            TempData["SuccessMessage"] = "Đặt lịch khám thành công!";
            return RedirectToAction("Appointments");
        }
        else
        {
            TempData["ErrorMessage"] = "Đặt lịch khám thất bại!";
            return RedirectToAction("BookAppointment");
        }
    }

    // GET /Patient/Appointments
    public async Task<IActionResult> Appointments()
    {
        if (!IsPatient())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var appointments = await _apiService.GetMyAppointmentsAsync();
        return View(appointments ?? new List<Appointment>());
    }

    // GET /Patient/GetTimeSlots
    public async Task<JsonResult> GetTimeSlots(int doctorId, string date)
    {
        var slots = await _apiService.GetAvailableTimeSlotsAsync(doctorId, date);
        return Json(slots);
    }

    // POST /Patient/CancelAppointment
    [HttpPost]
    public async Task<IActionResult> CancelAppointment(int id)
    {
        if (!IsPatient())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var success = await _apiService.CancelMyAppointmentAsync(id);
        if (success)
        {
            TempData["SuccessMessage"] = "Hủy lịch khám thành công!";
        }
        else
        {
            TempData["ErrorMessage"] = "Hủy lịch khám thất bại!";
        }

        return RedirectToAction("Appointments");
    }

    // GET /Patient/MedicalRecords
    public async Task<IActionResult> MedicalRecords()
    {
        if (!IsPatient())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var records = await _apiService.GetMyMedicalRecordsAsync();
        return View(records ?? new List<MedicalRecord>());
    }

    // GET /Patient/Prescriptions
    public async Task<IActionResult> Prescriptions()
    {
        if (!IsPatient())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var prescriptions = await _apiService.GetMyPrescriptionsAsync();
        return View(prescriptions ?? new List<Prescription>());
    }
}
