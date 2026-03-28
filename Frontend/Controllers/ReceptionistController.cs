using Microsoft.AspNetCore.Mvc;
using QuanLyBenhVien.Frontend.Services;
using QuanLyBenhVien.Frontend.Models;

namespace QuanLyBenhVien.Frontend.Controllers;

public class ReceptionistController : Controller
{
    private readonly ApiService _apiService;

    public ReceptionistController(ApiService apiService)
    {
        _apiService = apiService;
    }

    private bool IsReceptionist()
    {
        return HttpContext.Session.GetString("Role") == "RECEPTIONIST" || HttpContext.Session.GetString("Role") == "ADMIN";
    }

    // GET /Receptionist - Receptionist Dashboard
    public async Task<IActionResult> Index()
    {
        if (!IsReceptionist())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        ViewBag.FullName = HttpContext.Session.GetString("FullName");
        ViewBag.Role = "RECEPTIONIST";

        // Get today's queue
        var queue = await _apiService.GetTodayQueueAsync();
        ViewBag.Queue = queue ?? new List<QueueItem>();

        return View();
    }

    // GET /Receptionist/Queue
    public async Task<IActionResult> Queue()
    {
        if (!IsReceptionist())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var queue = await _apiService.GetTodayQueueAsync();
        return View(queue ?? new List<QueueItem>());
    }

    // POST /Receptionist/CheckIn
    [HttpPost]
    public async Task<IActionResult> CheckIn(int appointmentId)
    {
        if (!IsReceptionist())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var success = await _apiService.CheckInPatientAsync(appointmentId);
        if (success)
        {
            TempData["SuccessMessage"] = "Check-in thành công!";
        }
        else
        {
            TempData["ErrorMessage"] = "Check-in thất bại!";
        }

        return RedirectToAction("Queue");
    }

    // POST /Receptionist/CallPatient
    [HttpPost]
    public async Task<IActionResult> CallPatient(int appointmentId)
    {
        if (!IsReceptionist())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var success = await _apiService.CallPatientAsync(appointmentId);
        if (success)
        {
            TempData["SuccessMessage"] = "Đã gọi bệnh nhân!";
        }
        else
        {
            TempData["ErrorMessage"] = "Gọi bệnh nhân thất bại!";
        }

        return RedirectToAction("Queue");
    }

    // GET /Receptionist/Patients
    public async Task<IActionResult> Patients()
    {
        if (!IsReceptionist())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var patients = await _apiService.GetPatientsAsync();
        return View(patients ?? new List<Patient>());
    }

    // GET /Receptionist/Appointments
    public async Task<IActionResult> Appointments()
    {
        if (!IsReceptionist())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var appointments = await _apiService.GetAppointmentsAsync();
        return View(appointments ?? new List<Appointment>());
    }

    // GET /Receptionist/Emergency
    public async Task<IActionResult> Emergency()
    {
        if (!IsReceptionist())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var emergencies = await _apiService.GetEmergenciesAsync();
        return View(emergencies ?? new List<EmergencyRequest>());
    }
}
