using System.Web.Mvc;

namespace QuanLyBenhVien.Controllers
{
    public class HomeController : Controller
    {
        // Trang chủ hiển thị tổng quan hoặc dashboard
        public ActionResult Index()
        {
            ViewBag.Title = "Trang chủ - Quản lý bệnh viện";
            return View();
        }

        // Giới thiệu hệ thống, mục đích sử dụng
        public ActionResult About()
        {
            ViewBag.Title = "Giới thiệu";
            ViewBag.Message = "Hệ thống quản lý bệnh viện giúp tối ưu quy trình điều trị, đăng ký khám và lưu trữ bệnh án.";
            return View();
        }

        // Thông tin liên hệ bệnh viện
        public ActionResult Contact()
        {
            ViewBag.Title = "Liên hệ";
            ViewBag.Message = "Liên hệ với quản trị viên hoặc nhân viên hỗ trợ kỹ thuật của hệ thống bệnh viện.";
            return View();
        }
    }
}
