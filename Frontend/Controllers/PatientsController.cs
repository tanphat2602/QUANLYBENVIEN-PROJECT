using Microsoft.AspNetCore.Mvc;
using QuanLyBenhVien.Frontend.Services;

namespace QuanLyBenhVien.Frontend.Controllers;

public class PatientsController : Controller
{
    private readonly ApiService _apiService;

    public PatientsController(ApiService apiService)
    {
        _apiService = apiService;
    }

    private bool IsAdmin()
    {
        var role = HttpContext.Session.GetString("Role");
        return role == "Admin";
    }

    public IActionResult Index()
    {
        if (!IsAdmin())
        {
            return RedirectToAction("Login", "Account");
        }

        var patients = _apiService.GetPatientsAsync().Result;
        return View(patients);
    }
}
