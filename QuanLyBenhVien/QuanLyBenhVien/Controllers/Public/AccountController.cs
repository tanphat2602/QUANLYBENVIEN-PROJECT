using QuanLyBenhVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace QuanLyBenhVien.Controllers.Public
{
    public class AccountController : Controller
    {
        public QLBVDataContext db = new QLBVDataContext();

        
        public ActionResult Register()
        {
            using (QLBVDataContext context = new QLBVDataContext())
            {
                if (Request.HttpMethod == "POST")
                {
                    string username = Request.Form["Username"]?.Trim();
                    string email = Request.Form["Email"]?.Trim();
                    string password = Request.Form["PasswordHash"]?.Trim();
                    string fullName = Request.Form["FullName"]?.Trim();
                    string gender = Request.Form["Gender"]?.Trim();
                    string phone = Request.Form["Phone"]?.Trim();

                    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email) &&
                        !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(fullName))
                    {
                        if (password.Length < 8)
                        {
                            ViewBag.ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự!";
                            return View();
                        }

                        bool isEmailExists = context.Users.Any(u => u.Email == email);
                        if (isEmailExists)
                        {
                            ViewBag.ErrorMessage = "Email này đã được sử dụng!";
                            return View();
                        }

                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                        int newId = (context.Users.Any()) ? context.Users.Max(u => u.UserID) + 1 : 1;

                        var newUser = new User
                        {
                            UserID = newId,
                            Username = username,
                            Email = email,
                            PasswordHash = hashedPassword,
                            FullName = fullName,
                            Phone = phone,
                            Gender = gender,
                            DateOfBirth = null,
                            CreatedAt = DateTime.Now,
                            Status = "Hoạt động"
                        };

                        try
                        {
                            context.Users.InsertOnSubmit(newUser);
                            context.SubmitChanges();

                            // Gán vai trò mặc định là Patient (nếu có)
                            var role = context.Roles.FirstOrDefault(r => r.RoleName == "Patient");
                            if (role != null)
                            {
                                var userRole = new UserRole
                                {
                                    UserID = newUser.UserID,
                                    RoleID = role.RoleID
                                };
                                context.UserRoles.InsertOnSubmit(userRole);
                                context.SubmitChanges();
                            }

                            // Tạo record bệnh nhân
                            var patient = new Patient
                            {
                                UserID = newUser.UserID
                            };
                            context.Patients.InsertOnSubmit(patient);
                            context.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            ViewBag.ErrorMessage = "Lỗi khi thêm dữ liệu: " + ex.Message;
                            return View();
                        }

                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin!";
                    }
                }
            }

            return View();
        }
        public ActionResult Login()
        {
            if (Request.HttpMethod == "POST")
            {
                string username = Request.Form["Username"]?.Trim();
                string password = Request.Form["Password"]?.Trim();

                using (QLBVDataContext context = new QLBVDataContext())
                {
                    var user = context.Users.FirstOrDefault(u => u.Username == username);

                    if (user == null)
                    {
                        ViewBag.ErrorMessage = "Tài khoản không tồn tại!";
                        return View();
                    }

                    // Kiểm tra mật khẩu bằng BCrypt
                    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
                    if (!isPasswordValid)
                    {
                        ViewBag.ErrorMessage = "Mật khẩu không đúng!";
                        return View();
                    }

                    if (user.Status != "Hoạt động")
                    {
                        ViewBag.ErrorMessage = "Tài khoản đã bị khóa!";
                        return View();
                    }

                    // Lấy vai trò đầu tiên của người dùng
                    string roleName = context.UserRoles
                        .Where(r => r.UserID == user.UserID)
                        .Select(r => r.Role.RoleName)
                        .FirstOrDefault();

                    // Lưu session
                    Session["UserID"] = user.UserID;
                    Session["Username"] = user.Username;
                    Session["Role"] = roleName;

                    // Điều hướng theo quyền
                    switch (roleName)
                    {
                        case "Admin":
                            return RedirectToAction("Dashboard", "Admin");
                        case "Doctor":
                            return RedirectToAction("Index", "Doctor");
                        case "Nurse":
                            return RedirectToAction("Index", "Nurse");
                        case "Patient":
                            return RedirectToAction("Index", "Home");
                        default:
                            ViewBag.ErrorMessage = "Không xác định vai trò người dùng.";
                            return View();
                    }
                }
            }

            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

    }
}
