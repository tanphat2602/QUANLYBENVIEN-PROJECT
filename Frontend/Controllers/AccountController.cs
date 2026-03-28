using Microsoft.AspNetCore.Mvc;
using QuanLyBenhVien.Frontend.Models;
using QuanLyBenhVien.Frontend.Services;
using System.Text.Json;

namespace QuanLyBenhVien.Frontend.Controllers;

public class AccountController : Controller
{
    private readonly ApiService _apiService;

    public AccountController(ApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            var role = HttpContext.Session.GetString("Role");
            return RedirectToRoleDashboard(role);
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var response = await _apiService.LoginAsync(model.Username, model.Password);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(json);

            var token = result.GetProperty("token").GetString();
            var userId = result.GetProperty("userId").GetInt32();
            var username = result.GetProperty("username").GetString();
            var fullName = result.GetProperty("fullName").GetString();
            var role = result.GetProperty("role").GetString();

            // Store session data
            HttpContext.Session.SetString("Token", token ?? "");
            HttpContext.Session.SetInt32("UserID", userId);
            HttpContext.Session.SetString("Username", username ?? "");
            HttpContext.Session.SetString("FullName", fullName ?? "");
            HttpContext.Session.SetString("Role", role ?? "");

            // Store patient/doctor ID if available
            if (result.TryGetProperty("patientId", out var patientIdElement) && patientIdElement.ValueKind != JsonValueKind.Null)
            {
                HttpContext.Session.SetInt32("PatientID", patientIdElement.GetInt32());
            }
            if (result.TryGetProperty("doctorId", out var doctorIdElement) && doctorIdElement.ValueKind != JsonValueKind.Null)
            {
                HttpContext.Session.SetInt32("DoctorID", doctorIdElement.GetInt32());
            }

            // Store permissions if available
            if (result.TryGetProperty("permissions", out var permsElement) && permsElement.ValueKind == JsonValueKind.Array)
            {
                var permissions = permsElement.EnumerateArray().Select(p => p.GetString()).Where(p => p != null).ToList();
                HttpContext.Session.SetString("Permissions", JsonSerializer.Serialize(permissions));
            }

            return RedirectToRoleDashboard(role);
        }
        else
        {
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(json);
            ViewBag.ErrorMessage = result.GetProperty("message").GetString();
            return View();
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        var response = await _apiService.RegisterAsync(model);

        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }
        else
        {
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(json);
            ViewBag.ErrorMessage = result.GetProperty("message").GetString();
            return View();
        }
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    private IActionResult RedirectToRoleDashboard(string? role)
    {
        return role?.ToUpperInvariant() switch
        {
            "ADMIN" => RedirectToAction("Dashboard", "Admin"),
            "DOCTOR" => RedirectToAction("Index", "Doctor"),
            "PATIENT" => RedirectToAction("Index", "Patient"),
            "RECEPTIONIST" => RedirectToAction("Index", "Receptionist"),
            "PHARMACIST" => RedirectToAction("Index", "Pharmacist"),
            "NURSE" => RedirectToAction("Index", "Nurse"),
            _ => RedirectToAction("Login")
        };
    }
}
