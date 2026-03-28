using Microsoft.AspNetCore.Mvc;
using QuanLyBenhVien.Frontend.Services;
using QuanLyBenhVien.Frontend.Models;

namespace QuanLyBenhVien.Frontend.Controllers;

public class PharmacistController : Controller
{
    private readonly ApiService _apiService;

    public PharmacistController(ApiService apiService)
    {
        _apiService = apiService;
    }

    private bool IsPharmacist()
    {
        return HttpContext.Session.GetString("Role") == "PHARMACIST" || HttpContext.Session.GetString("Role") == "ADMIN";
    }

    // GET /Pharmacist - Pharmacist Dashboard
    public async Task<IActionResult> Index()
    {
        if (!IsPharmacist())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        ViewBag.FullName = HttpContext.Session.GetString("FullName");
        ViewBag.Role = "PHARMACIST";

        // Get pending prescriptions
        var pending = await _apiService.GetPendingPrescriptionsAsync();
        ViewBag.PendingPrescriptions = pending ?? new List<object>();

        return View();
    }

    // GET /Pharmacist/Medicines
    public async Task<IActionResult> Medicines()
    {
        if (!IsPharmacist())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var medicines = await _apiService.GetMedicinesWithStockAsync();
        return View(medicines ?? new List<Medicine>());
    }

    // GET /Pharmacist/PendingPrescriptions
    public async Task<IActionResult> PendingPrescriptions()
    {
        if (!IsPharmacist())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var prescriptions = await _apiService.GetPendingPrescriptionsAsync();
        return View(prescriptions ?? new List<object>());
    }

    // POST /Pharmacist/Dispense
    [HttpPost]
    public async Task<IActionResult> Dispense(int prescriptionId)
    {
        if (!IsPharmacist())
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var success = await _apiService.DispensePrescriptionAsync(prescriptionId);
        if (success)
        {
            TempData["SuccessMessage"] = "Cấp phát thuốc thành công!";
        }
        else
        {
            TempData["ErrorMessage"] = "Cấp phát thuốc thất bại!";
        }

        return RedirectToAction("PendingPrescriptions");
    }
}
