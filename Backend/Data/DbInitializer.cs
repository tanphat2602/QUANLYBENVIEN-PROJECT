using BCrypt.Net;
using QuanLyBenhVien.API.Models;

namespace QuanLyBenhVien.API.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (context.Users.Any())
        {
            return;
        }

        // ===== ROLES =====
        var roles = new List<Role>
        {
            new Role { RoleName = "Admin", Description = "Quản trị hệ thống" },
            new Role { RoleName = "Doctor", Description = "Bác sĩ" },
            new Role { RoleName = "Nurse", Description = "Y tá / Điều dưỡng" },
            new Role { RoleName = "Patient", Description = "Bệnh nhân" }
        };
        context.Roles.AddRange(roles);
        context.SaveChanges();

        // ===== ADMIN (password: 1) =====
        var adminPassword = BCrypt.Net.BCrypt.HashPassword("1");
        var admin = new User
        {
            Username = "admin",
            PasswordHash = adminPassword,
            Email = "admin@hospital.com",
            FullName = "Quản trị viên",
            Phone = "0901234567",
            Gender = "Nam",
            DateOfBirth = new DateTime(1990, 1, 1),
            Address = "123 Đường ABC, Quận 1, TP.HCM",
            Status = "Hoạt động",
            CreatedAt = DateTime.Now
        };
        context.Users.Add(admin);
        context.SaveChanges();
        context.UserRoles.Add(new UserRole { UserID = admin.UserID, RoleID = 1 });
        context.SaveChanges();

        // ===== DEPARTMENTS =====
        var departments = new List<Department>
        {
            new Department { DepartmentName = "Nội tổng hợp", Description = "Khám và điều trị các bệnh nội khoa", Location = "Tầng 2", Status = "Hoạt động" },
            new Department { DepartmentName = "Ngoại tổng hợp", Description = "Phẫu thuật và điều trị ngoại khoa", Location = "Tầng 3", Status = "Hoạt động" },
            new Department { DepartmentName = "Nhi khoa", Description = "Chăm sóc sức khỏe trẻ em", Location = "Tầng 4", Status = "Hoạt động" },
            new Department { DepartmentName = "Sản phụ khoa", Description = "Sản khoa và phụ khoa", Location = "Tầng 5", Status = "Hoạt động" },
            new Department { DepartmentName = "Tim mạch", Description = "Tim mạch và can thiệp tim", Location = "Tầng 6", Status = "Hoạt động" },
            new Department { DepartmentName = "Thần kinh", Description = "Thần kinh và đột quỵ", Location = "Tầng 7", Status = "Hoạt động" },
            new Department { DepartmentName = "Tai mũi họng", Description = "Tai mũi họng", Location = "Tầng 8", Status = "Hoạt động" },
            new Department { DepartmentName = "Mắt", Description = "Mắt và điều trị thị lực", Location = "Tầng 9", Status = "Hoạt động" },
            new Department { DepartmentName = "Da liễu", Description = "Da liễu và hoa liễu", Location = "Tầng 10", Status = "Hoạt động" },
            new Department { DepartmentName = "Cấp cứu", Description = "Cấp cứu 24/7", Location = "Tầng 1", Status = "Hoạt động" }
        };
        context.Departments.AddRange(departments);
        context.SaveChanges();

        // ===== ROOMS =====
        var rooms = new List<Room>
        {
            new Room { RoomNumber = "201", DepartmentID = 1, RoomType = "Phòng khám", Status = "Hoạt động" },
            new Room { RoomNumber = "202", DepartmentID = 1, RoomType = "Phòng khám", Status = "Hoạt động" },
            new Room { RoomNumber = "301", DepartmentID = 2, RoomType = "Phòng mổ", Status = "Hoạt động" },
            new Room { RoomNumber = "302", DepartmentID = 2, RoomType = "Phòng khám", Status = "Hoạt động" },
            new Room { RoomNumber = "401", DepartmentID = 3, RoomType = "Phòng khám", Status = "Hoạt động" },
            new Room { RoomNumber = "501", DepartmentID = 4, RoomType = "Phòng khám", Status = "Hoạt động" },
            new Room { RoomNumber = "601", DepartmentID = 5, RoomType = "Phòng khám", Status = "Hoạt động" },
            new Room { RoomNumber = "701", DepartmentID = 6, RoomType = "Phòng khám", Status = "Hoạt động" },
            new Room { RoomNumber = "E101", DepartmentID = 10, RoomType = "Cấp cứu", Status = "Hoạt động" },
            new Room { RoomNumber = "E102", DepartmentID = 10, RoomType = "Cấp cứu", Status = "Hoạt động" }
        };
        context.Rooms.AddRange(rooms);
        context.SaveChanges();

        // ===== DOCTOR (password: doctor) =====
        var doctorPassword = BCrypt.Net.BCrypt.HashPassword("doctor");
        var doctorUser = new User
        {
            Username = "doctor",
            PasswordHash = doctorPassword,
            Email = "dr.nguyenvana@hospital.com",
            FullName = "Bs. Nguyễn Văn A",
            Phone = "0902345678",
            Gender = "Nam",
            DateOfBirth = new DateTime(1980, 5, 15),
            Status = "Hoạt động",
            CreatedAt = DateTime.Now
        };
        context.Users.Add(doctorUser);
        context.SaveChanges();
        context.UserRoles.Add(new UserRole { UserID = doctorUser.UserID, RoleID = 2 });
        context.SaveChanges();

        var doctor = new Doctor
        {
            UserID = doctorUser.UserID,
            DepartmentID = 1,
            RoomID = 201,
            Specialty = "Nội tổng hợp",
            Experience = "15 năm kinh nghiệm",
            Education = "Đại học Y dược TP.HCM",
            ConsultationFee = 200000,
            WorkingSchedule = "Thứ 2 - Thứ 6: 8:00 - 17:00",
            Status = "Hoạt động"
        };
        context.Doctors.Add(doctor);
        context.SaveChanges();

        // Doctor Schedule
        var schedules = new List<DoctorSchedule>
        {
            new DoctorSchedule { DoctorID = doctor.DoctorID, DayOfWeek = "Thứ 2", Shift = "Sáng", TimeStart = "08:00", TimeEnd = "12:00", MaxPatients = 20, CurrentPatients = 0, Status = "Hoạt động" },
            new DoctorSchedule { DoctorID = doctor.DoctorID, DayOfWeek = "Thứ 2", Shift = "Chiều", TimeStart = "13:00", TimeEnd = "17:00", MaxPatients = 15, CurrentPatients = 0, Status = "Hoạt động" },
            new DoctorSchedule { DoctorID = doctor.DoctorID, DayOfWeek = "Thứ 3", Shift = "Sáng", TimeStart = "08:00", TimeEnd = "12:00", MaxPatients = 20, CurrentPatients = 0, Status = "Hoạt động" },
            new DoctorSchedule { DoctorID = doctor.DoctorID, DayOfWeek = "Thứ 4", Shift = "Sáng", TimeStart = "08:00", TimeEnd = "12:00", MaxPatients = 20, CurrentPatients = 0, Status = "Hoạt động" },
            new DoctorSchedule { DoctorID = doctor.DoctorID, DayOfWeek = "Thứ 5", Shift = "Sáng", TimeStart = "08:00", TimeEnd = "12:00", MaxPatients = 20, CurrentPatients = 0, Status = "Hoạt động" },
            new DoctorSchedule { DoctorID = doctor.DoctorID, DayOfWeek = "Thứ 6", Shift = "Sáng", TimeStart = "08:00", TimeEnd = "12:00", MaxPatients = 20, CurrentPatients = 0, Status = "Hoạt động" }
        };
        context.DoctorSchedules.AddRange(schedules);
        context.SaveChanges();

        // ===== MEDICINES =====
        var medicines = new List<Medicine>
        {
            new Medicine { MedicineName = "Paracetamol 500mg", GenericName = "Acetaminophen", Manufacturer = "Stada", Unit = "Viên", Price = 500, Stock = 10000, DosageForm = "Viên nén", Status = "Hoạt động" },
            new Medicine { MedicineName = "Amoxicillin 500mg", GenericName = "Amoxicillin", Manufacturer = "Domesco", Unit = "Viên", Price = 800, Stock = 5000, DosageForm = "Viên nang", Status = "Hoạt động" },
            new Medicine { MedicineName = "Ibuprofen 400mg", GenericName = "Ibuprofen", Manufacturer = "Berlin", Unit = "Viên", Price = 1000, Stock = 3000, DosageForm = "Viên nén", Status = "Hoạt động" },
            new Medicine { MedicineName = "Vitamin C 500mg", GenericName = "Ascorbic acid", Manufacturer = "Pharmacity", Unit = "Viên", Price = 300, Stock = 15000, DosageForm = "Viên sủi", Status = "Hoạt động" },
            new Medicine { MedicineName = "Omeprazole 20mg", GenericName = "Omeprazole", Manufacturer = "Daewoong", Unit = "Viên", Price = 2000, Stock = 2000, DosageForm = "Viên nang", Status = "Hoạt động" },
            new Medicine { MedicineName = "Metformin 500mg", GenericName = "Metformin", Manufacturer = "DKSH", Unit = "Viên", Price = 500, Stock = 3000, DosageForm = "Viên nén", Status = "Hoạt động" },
            new Medicine { MedicineName = "Amlodipine 5mg", GenericName = "Amlodipine", Manufacturer = "Pfizer", Unit = "Viên", Price = 1500, Stock = 2000, DosageForm = "Viên nén", Status = "Hoạt động" },
            new Medicine { MedicineName = "Cetirizine 10mg", GenericName = "Cetirizine", Manufacturer = "UCB", Unit = "Viên", Price = 800, Stock = 2500, DosageForm = "Viên nén", Status = "Hoạt động" },
            new Medicine { MedicineName = "Dexamethasone 0.5mg", GenericName = "Dexamethasone", Manufacturer = "DKSH", Unit = "Viên", Price = 500, Stock = 1000, DosageForm = "Viên nén", Status = "Hoạt động" },
            new Medicine { MedicineName = "Salbutamol 2mg", GenericName = "Salbutamol", Manufacturer = "GSK", Unit = "Viên", Price = 600, Stock = 1500, DosageForm = "Viên nén", Status = "Hoạt động" }
        };
        context.Medicines.AddRange(medicines);
        context.SaveChanges();

        // ===== MEDICAL SERVICES =====
        var services = new List<MedicalService>
        {
            new MedicalService { ServiceName = "Khám tổng quát", Description = "Khám sức khỏe tổng quát", Price = 150000, DepartmentID = 1, Status = "Hoạt động" },
            new MedicalService { ServiceName = "Khám chuyên khoa", Description = "Khám chuyên khoa", Price = 200000, DepartmentID = 1, Status = "Hoạt động" },
            new MedicalService { ServiceName = "Xét nghiệm máu", Description = "Công thức máu, sinh hóa", Price = 100000, DepartmentID = 1, Status = "Hoạt động" },
            new MedicalService { ServiceName = "X-quang", Description = "Chụp X-quang", Price = 150000, DepartmentID = 2, Status = "Hoạt động" },
            new MedicalService { ServiceName = "Siêu âm", Description = "Siêu âm 4D", Price = 250000, DepartmentID = 1, Status = "Hoạt động" },
            new MedicalService { ServiceName = "Điện tim đồ", Description = "ECG", Price = 120000, DepartmentID = 5, Status = "Hoạt động" },
            new MedicalService { ServiceName = "Nội soi", Description = "Nội soi tiêu hóa", Price = 500000, DepartmentID = 1, Status = "Hoạt động" }
        };
        context.MedicalServices.AddRange(services);
        context.SaveChanges();

        // ===== INSURANCE =====
        var insurance = new Insurance
        {
            InsuranceProvider = "Bảo hiểm Y tế Quốc gia",
            PolicyNumber = "BHYT123456789",
            ExpiryDate = DateTime.Now.AddYears(2),
            CoverageAmount = 80000000
        };
        context.Insurances.Add(insurance);
        context.SaveChanges();

        // ===== PATIENT (password: patient) =====
        var patientPassword = BCrypt.Net.BCrypt.HashPassword("patient");
        var patientUser = new User
        {
            Username = "patient",
            PasswordHash = patientPassword,
            Email = "patient@email.com",
            FullName = "Trần Thị B",
            Phone = "0903456789",
            Gender = "Nữ",
            DateOfBirth = new DateTime(1995, 8, 20),
            Address = "456 Đường XYZ, Quận 2, TP.HCM",
            Status = "Hoạt động",
            CreatedAt = DateTime.Now
        };
        context.Users.Add(patientUser);
        context.SaveChanges();
        context.UserRoles.Add(new UserRole { UserID = patientUser.UserID, RoleID = 4 });
        context.SaveChanges();

        var patient = new Patient
        {
            UserID = patientUser.UserID,
            InsuranceID = insurance.InsuranceID,
            BloodType = "O",
            Height = 160,
            Weight = 55,
            Status = "Hoạt động",
            CreatedAt = DateTime.Now
        };
        context.Patients.Add(patient);
        context.SaveChanges();

        // ===== SAMPLE APPOINTMENT =====
        var appointment = new Appointment
        {
            PatientID = patient.PatientID,
            DoctorID = doctor.DoctorID,
            DepartmentID = 1,
            AppointmentDate = DateTime.Now.AddDays(2),
            TimeSlot = "08:00 - 08:30",
            Status = "Chờ khám",
            Reason = "Đau đầu, hoa mắt",
            AppointmentType = "Khám tổng quát",
            IsEmergency = false,
            Priority = 1,
            CreatedAt = DateTime.Now
        };
        context.Appointments.Add(appointment);
        context.SaveChanges();
    }
}
