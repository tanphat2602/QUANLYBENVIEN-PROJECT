using QuanLyBenhVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBenhVien.Controllers.Admin
{
    public class AdminController : Controller
    {
        // GET: Admin
        QLBVDataContext db = new QLBVDataContext();

        // Trang tổng quan chỉ dành cho Admin
        public ActionResult Dashboard()
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            // Truy xuất dữ liệu thống kê mẫu
            ViewBag.TotalUsers = db.Users.Count();
            ViewBag.TotalDoctors = db.UserRoles.Count(r => r.Role.RoleName == "Doctor");
            ViewBag.TotalPatients = db.UserRoles.Count(r => r.Role.RoleName == "Patient");

            return View();
        }
    }
}