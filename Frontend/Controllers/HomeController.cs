using Microsoft.AspNetCore.Mvc;

namespace QuanLyBenhVien.Frontend.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        ViewData["Message"] = "Hệ thống quản lý bệnh viện";
        return View();
    }

    public IActionResult Contact()
    {
        ViewData["Message"] = "Liên hệ hỗ trợ";
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
