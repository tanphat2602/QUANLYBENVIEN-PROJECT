using QuanLyBenhVien.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyBenhVien.Controllers
{
    public class DoctorController : Controller
    {
        private QLBVDataContext db = new QLBVDataContext();

        // Trang chính sau khi Doctor đăng nhập
        public ActionResult Index()
        {
            // Lấy thông tin Doctor từ Session
            if (Session["UserID"] == null || Session["Role"]?.ToString() != "Doctor")
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(Session["UserID"]);
            var doctor = db.Doctors.FirstOrDefault(d => d.UserID == userId);

            if (doctor == null)
            {
                ViewBag.ErrorMessage = "Không tìm thấy thông tin bác sĩ.";
                return RedirectToAction("Login", "Account");
            }

            return View(doctor);
        }

        // Xem danh sách bệnh nhân của bác sĩ
       

        // Xem lịch hẹn của bác sĩ
        public ActionResult Appointments()
        {
            if (Session["UserID"] == null || Session["Role"]?.ToString() != "Doctor")
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(Session["UserID"]);
            var doctor = db.Doctors.FirstOrDefault(d => d.UserID == userId);

            if (doctor == null)
            {
                ViewBag.ErrorMessage = "Không tìm thấy thông tin bác sĩ.";
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách lịch hẹn của bác sĩ
            var appointments = db.Appointments.Where(a => a.DoctorID == doctor.DoctorID).ToList();

            return View(appointments);
        }

        // Xem thông tin chi tiết bác sĩ
        public ActionResult Details(int id)
        {
            if (Session["UserID"] == null || Session["Role"]?.ToString() != "Doctor")
            {
                return RedirectToAction("Login", "Account");
            }

            var doctor = db.Doctors.FirstOrDefault(d => d.DoctorID == id);
            if (doctor == null)
            {
                return HttpNotFound();
            }

            return View(doctor);
        }

        // Chỉnh sửa thông tin bác sĩ
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["UserID"] == null || Session["Role"]?.ToString() != "Doctor")
            {
                return RedirectToAction("Login", "Account");
            }

            var doctor = db.Doctors.FirstOrDefault(d => d.DoctorID == id);
            if (doctor == null)
            {
                return HttpNotFound();
            }

            return View(doctor);
        }

        [HttpPost]
        public ActionResult Edit(Doctor doctor)
        {
            if (Session["UserID"] == null || Session["Role"]?.ToString() != "Doctor")
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingDoctor = db.Doctors.FirstOrDefault(d => d.DoctorID == doctor.DoctorID);
                    if (existingDoctor != null)
                    {
                        existingDoctor.DepartmentID = doctor.DepartmentID;
                        existingDoctor.Specialty = doctor.Specialty;
                        existingDoctor.WorkingSchedule = doctor.WorkingSchedule;

                        db.SubmitChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Lỗi khi cập nhật thông tin bác sĩ: " + ex.Message;
                }
            }

            return View(doctor);
        }
    }
}
