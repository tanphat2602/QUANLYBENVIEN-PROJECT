using Microsoft.AspNetCore.Mvc;
using QuanLyBenhVien.Frontend.Models;
using QuanLyBenhVien.Frontend.Services;

namespace QuanLyBenhVien.Frontend.Controllers;

public class DoctorController : Controller
{
    private readonly ApiService _apiService;

    public DoctorController(ApiService apiService)
    {
        _apiService = apiService;
    }

    private bool IsDoctor()
    {
        var role = HttpContext.Session.GetString("Role");
        return role == "DOCTOR" || role == "Doctor";
    }

    public IActionResult Index()
    {
        if (!IsDoctor())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        ViewBag.FullName = HttpContext.Session.GetString("FullName");
        ViewBag.Role = HttpContext.Session.GetString("Role");
        return View();
    }

    public async Task<IActionResult> Appointments()
    {
        if (!IsDoctor())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var appointments = await _apiService.GetDoctorScheduleAsync();
        return View(appointments ?? new List<Appointment>());
    }

    public async Task<IActionResult> Patients()
    {
        if (!IsDoctor())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var patients = await _apiService.GetPatientsAsync();
        return View(patients ?? new List<Patient>());
    }
}
