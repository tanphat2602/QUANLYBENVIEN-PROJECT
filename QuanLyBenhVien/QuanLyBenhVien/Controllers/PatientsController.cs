using QuanLyBenhVien.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyBenhVien.Controllers.Admin
{
    public class PatientsController : Controller
    {
        private QLBVDataContext db = new QLBVDataContext();

        // Danh sách bệnh nhân
        public ActionResult Index()
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var patients = db.Patients.ToList();
            return View(patients);
        }

        // Chi tiết bệnh nhân
        public ActionResult Details(int id)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var patient = db.Patients.FirstOrDefault(p => p.PatientID == id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            return View(patient);
        }

        // Tạo bệnh nhân mới
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Create(Patient patient)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Patients.InsertOnSubmit(patient);
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Lỗi khi thêm bệnh nhân: " + ex.Message;
                }
            }

            return View(patient);
        }

        // Chỉnh sửa thông tin bệnh nhân
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var patient = db.Patients.FirstOrDefault(p => p.PatientID == id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            return View(patient);
        }

        [HttpPost]
        public ActionResult Edit(Patient patient)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPatient = db.Patients.FirstOrDefault(p => p.PatientID == patient.PatientID);
                    if (existingPatient != null)
                    {
                        existingPatient.UserID = patient.UserID;
                        existingPatient.Address = patient.Address;
                        existingPatient.InsuranceID = patient.InsuranceID;

                        db.SubmitChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Lỗi khi cập nhật bệnh nhân: " + ex.Message;
                }
            }

            return View(patient);
        }

        // Xóa bệnh nhân
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var patient = db.Patients.FirstOrDefault(p => p.PatientID == id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            return View(patient);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var patient = db.Patients.FirstOrDefault(p => p.PatientID == id);
                if (patient != null)
                {
                    db.Patients.DeleteOnSubmit(patient);
                    db.SubmitChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Lỗi khi xóa bệnh nhân: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
