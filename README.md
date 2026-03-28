# HỆ THỐNG QUẢN LÝ BỆNH VIỆN - MedCare Hospital

## 📋 Mục lục
1. [Giới thiệu](#giới-thiệu)
2. [Công nghệ sử dụng](#công-nghệ-sử-dụng)
3. [Cấu trúc dự án](#cấu-trúc-dự-án)
4. [Hướng dẫn cài đặt](#hướng-dẫn-cài-đặt)
5. [Tài khoản đăng nhập mặc định](#tài-khoản-đăng-nhập-mặc-định)
6. [Chức năng theo vai trò](#chức-năng-theo-vai-trò)
7. [API Endpoints](#api-endpoints)
8. [Database Schema](#database-schema)

---

## 🏥 Giới thiệu

**MedCare Hospital** là hệ thống quản lý bệnh viện hiện đại, được xây dựng theo kiến trúc **Microservices** với:

- **Frontend**: ASP.NET Core MVC (.NET 8)
- **Backend**: Spring Boot (Java 17)
- **Database**: H2 In-Memory Database
- **Authentication**: JWT (JSON Web Token)
- **Authorization**: RBAC (Role-Based Access Control)

### Các tính năng chính:
- ✅ Đặt lịch khám online
- ✅ Quản lý bệnh nhân, bác sĩ, thuốc
- ✅ Quản lý lịch hẹn, hàng đợi
- ✅ Hồ sơ bệnh án điện tử
- ✅ Kê đơn thuốc và cấp phát thuốc
- ✅ Thanh toán viện phí
- ✅ Quản lý cấp cứu
- ✅ Dashboard thống kê

---

## 💻 Công nghệ sử dụng

### Backend (Spring Boot)
| Thành phần | Công nghệ |
|------------|-----------|
| Framework | Spring Boot 3.2.0 |
| Ngôn ngữ | Java 17 |
| ORM | Spring Data JPA / Hibernate 6.3 |
| Database | H2 Database |
| Security | Spring Security + JWT |
| API | RESTful API |
| Build Tool | Maven |

### Frontend (ASP.NET Core)
| Thành phần | Công nghệ |
|------------|-----------|
| Framework | ASP.NET Core MVC |
| Ngôn ngữ | C# / .NET 8 |
| UI | Bootstrap 5, Razor Views |
| Icons | Font Awesome 6.4 |
| Fonts | Google Fonts (Poppins) |
| HTTP Client | HttpClient Factory |

---

## 📁 Cấu trúc dự án

```
QuanLyBenhVien/
│
├── SpringBoot/                    # Backend (Java Spring Boot)
│   ├── src/main/java/com/hospital/qlbv/
│   │   ├── config/                # Cấu hình (Security, Data Initializer)
│   │   ├── controller/            # REST API Controllers
│   │   │   ├── AuthController.java
│   │   │   ├── AdminPortalController.java
│   │   │   ├── DoctorPortalController.java
│   │   │   ├── PatientPortalController.java
│   │   │   ├── PharmacistPortalController.java
│   │   │   ├── ReceptionistPortalController.java
│   │   │   └── ...
│   │   ├── dto/                    # Data Transfer Objects
│   │   ├── entity/                 # JPA Entities (Database Models)
│   │   ├── repository/             # Spring Data JPA Repositories
│   │   ├── security/               # JWT Authentication
│   │   └── service/                # Business Logic Services
│   ├── src/main/resources/
│   │   └── application.properties   # Cấu hình ứng dụng
│   └── pom.xml                     # Maven dependencies
│
├── Frontend/                       # Frontend (ASP.NET Core)
│   ├── Controllers/                 # MVC Controllers
│   │   ├── AccountController.cs    # Đăng nhập/đăng ký
│   │   ├── AdminController.cs      # Trang quản trị
│   │   ├── DoctorController.cs     # Trang bác sĩ
│   │   ├── PatientController.cs    # Trang bệnh nhân
│   │   ├── PharmacistController.cs # Trang dược sĩ
│   │   ├── ReceptionistController.cs # Trang lễ tân
│   │   └── ...
│   ├── Models/                      # View Models
│   ├── Services/                    # API Service
│   ├── Views/                       # Razor Views
│   │   ├── Account/
│   │   ├── Admin/
│   │   ├── Doctor/
│   │   ├── Home/
│   │   ├── Patient/
│   │   ├── Pharmacist/
│   │   ├── Receptionist/
│   │   └── Shared/
│   └── appsettings.json
│
└── README.md                        # Documentation
```

---

## 🚀 Hướng dẫn cài đặt

### Yêu cầu hệ thống
- **Java**: JDK 17+
- **.NET**: .NET 8 SDK
- **Maven**: 3.6+
- **Git**: Optional (để clone project)

### Cách 1: Chạy trực tiếp (Đã chạy sẵn)

**Bước 1: Chạy Backend (Spring Boot)**
```bash
cd SpringBoot
mvn spring-boot:run
# Hoặc chạy file JAR đã build sẵn
```

**Bước 2: Chạy Frontend (ASP.NET Core)**
```bash
cd Frontend
dotnet run --urls "http://0.0.0.0:5001"
```

**Bước 3: Truy cập ứng dụng**
- Frontend: http://localhost:5001
- Backend API: http://localhost:5000
- H2 Console: http://localhost:5000/h2-console

### Cách 2: Build JAR và chạy

**Build Backend:**
```bash
cd SpringBoot
mvn clean package -DskipTests
java -jar target/qlbv-1.0.0.jar
```

**Build Frontend:**
```bash
cd Frontend
dotnet publish -c Release
dotnet bin/Release/net8.0/publish/QuanLyBenhVien.Frontend.dll --urls "http://0.0.0.0:5001"
```

---

## 🔐 Tài khoản đăng nhập mặc định

### Admin (Quản trị viên)
| Thông tin | Giá trị |
|-----------|---------|
| Username | `admin` |
| Password | `1` |

### Bác sĩ (10 tài khoản)
| Username | Password |
|----------|----------|
| `dr.nguyenvana` | `123456` |
| `dr.tranthib` | `123456` |
| `dr.levanc` | `123456` |
| `dr.phamthid` | `123456` |
| `dr.hoangvane` | `123456` |
| `dr.nguyenthif` | `123456` |
| `dr.tranvanh` | `123456` |
| `dr.levani` | `123456` |
| `dr.phamthik` | `123456` |
| `dr.hoangthil` | `123456` |

### Lễ tân (2 tài khoản)
| Username | Password |
|----------|----------|
| `receptionist1` | `123456` |
| `receptionist2` | `123456` |

### Dược sĩ (2 tài khoản)
| Username | Password |
|----------|----------|
| `pharmacist1` | `123456` |
| `pharmacist2` | `123456` |

### Bệnh nhân (30 tài khoản)
| Username | Password |
|----------|----------|
| `patient1` - `patient30` | `123456` |

---

## 👥 Chức năng theo vai trò

### 1️⃣ ADMIN - Quản trị viên

**Dashboard:**
- Thống kê tổng quan (bệnh nhân, bác sĩ, lịch hẹn, doanh thu)
- Biểu đồ thống kê

**Quản lý người dùng:**
- Xem danh sách tất cả users
- Thêm/Sửa/Xóa users
- Phân quyền users

**Quản lý bác sĩ:**
- Xem danh sách bác sĩ
- Thêm/Sửa thông tin bác sĩ
- Phân công khoa/phòng

**Quản lý bệnh nhân:**
- Xem danh sách bệnh nhân
- Thông tin bảo hiểm, chiều cao, cân nặng

**Quản lý thuốc:**
- Xem kho thuốc
- Thêm/Sửa thông tin thuốc

**Quản lý lịch hẹn:**
- Xem tất cả lịch hẹn
- Cập nhật trạng thái

**Quản lý cấp cứu:**
- Xem danh sách cấp cứu
- Phân công bác sĩ

---

### 2️⃣ DOCTOR - Bác sĩ

**Trang chủ:**
- Lịch khám hôm nay
- Số bệnh nhân chờ khám

**Lịch khám:**
- Xem lịch hẹn của mình
- Cập nhật trạng thái (Đang khám, Hoàn thành)

**Bệnh nhân:**
- Xem danh sách bệnh nhân đã khám
- Xem hồ sơ bệnh án
- Tạo/Sửa hồ sơ bệnh án
- Kê đơn thuốc

---

### 3️⃣ PATIENT - Bệnh nhân

**Trang chủ:**
- Thông tin cá nhân
- Lịch hẹn sắp tới

**Đặt lịch khám:**
- Chọn khoa khám
- Chọn bác sĩ
- Chọn ngày và giờ
- Chọn loại khám (Khám bệnh, Tái khám, Khám định kỳ, Khám chuyên khoa)
- Nhập lý do khám

**Lịch hẹn:**
- Xem danh sách lịch hẹn
- Hủy lịch hẹn (nếu chưa được xác nhận)

**Hồ sơ bệnh án:**
- Xem lịch sử khám bệnh
- Xem chi tiết bệnh án

**Đơn thuốc:**
- Xem đơn thuốc đã được kê
- Trạng thái đơn thuốc

---

### 4️⃣ RECEPTIONIST - Lễ tân

**Trang chủ:**
- Số bệnh nhân trong ngày
- Lịch hẹn hôm nay

**Hàng đợi:**
- Danh sách bệnh nhân chờ
- Check-in bệnh nhân
- Gọi bệnh nhân tiếp theo
- Cập nhật trạng thái

**Quản lý bệnh nhân:**
- Thêm bệnh nhân mới
- Sửa thông tin bệnh nhân

**Quản lý lịch hẹn:**
- Tạo lịch hẹn cho bệnh nhân
- Sửa/Hủy lịch hẹn

**Cấp cứu:**
- Tiếp nhận bệnh nhân cấp cứu
- Gán bác sĩ phụ trách

---

### 5️⃣ PHARMACIST - Dược sĩ

**Trang chủ:**
- Số đơn thuốc chờ
- Thuốc sắp hết hàng

**Kho thuốc:**
- Xem danh sách thuốc
- Thêm/Sửa thông tin thuốc
- Cập nhật số lượng tồn kho

**Đơn thuốc chờ:**
- Danh sách đơn thuốc chưa cấp
- Xem chi tiết đơn thuốc
- Cấp phát thuốc (xác nhận đã phát)

---

### 6️⃣ NURSE - Y tá (Chỉ có quyền xem)

- Xem danh sách bệnh nhân
- Xem hồ sơ bệnh án
- Check-in bệnh nhân
- Cập nhật thông tin bệnh án

---

## 🌐 API Endpoints

### Authentication
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| POST | `/api/auth/login` | Đăng nhập |
| POST | `/api/auth/register` | Đăng ký |

### Dashboard
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/dashboard/stats` | Thống kê dashboard |

### Users (Admin)
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/users` | Danh sách users |
| POST | `/api/users` | Tạo user |
| PUT | `/api/users/{id}` | Sửa user |
| DELETE | `/api/users/{id}` | Xóa user |

### Doctors
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/doctors` | Danh sách bác sĩ |
| GET | `/api/doctors/{id}` | Chi tiết bác sĩ |
| POST | `/api/doctors` | Tạo bác sĩ |
| PUT | `/api/doctors/{id}` | Sửa bác sĩ |
| DELETE | `/api/doctors/{id}` | Xóa bác sĩ |

### Patients
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/patients` | Danh sách bệnh nhân |
| GET | `/api/patients/{id}` | Chi tiết bệnh nhân |
| POST | `/api/patients` | Tạo bệnh nhân |
| PUT | `/api/patients/{id}` | Sửa bệnh nhân |
| DELETE | `/api/patients/{id}` | Xóa bệnh nhân |

### Departments
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/departments` | Danh sách khoa |

### Appointments
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/appointments` | Danh sách lịch hẹn |
| POST | `/api/appointments` | Tạo lịch hẹn |
| DELETE | `/api/appointments/{id}` | Hủy lịch hẹn |

### Medicines
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/medicines` | Danh sách thuốc |

### Patient Portal
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/patient/profile` | Hồ sơ bệnh nhân |
| PUT | `/api/patient/profile` | Cập nhật hồ sơ |
| GET | `/api/patient/appointments` | Lịch hẹn của tôi |
| POST | `/api/patient/appointments` | Đặt lịch hẹn |
| DELETE | `/api/patient/appointments/{id}` | Hủy lịch hẹn |
| GET | `/api/patient/medical-records` | Hồ sơ bệnh án |
| GET | `/api/patient/prescriptions` | Đơn thuốc |
| GET | `/api/patient/time-slots` | Lấy khung giờ trống |

### Doctor Portal
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/doctor/profile` | Hồ sơ bác sĩ |
| GET | `/api/doctor/schedule` | Lịch làm việc |
| GET | `/api/doctor/patient/{id}` | Chi tiết bệnh nhân |
| POST | `/api/doctor/medical-record` | Tạo bệnh án |
| POST | `/api/doctor/prescription` | Kê đơn thuốc |

### Receptionist Portal
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/receptionist/queue` | Hàng đợi hôm nay |
| POST | `/api/receptionist/checkin` | Check-in bệnh nhân |
| PUT | `/api/receptionist/appointment/{id}/call` | Gọi bệnh nhân |

### Pharmacist Portal
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/pharmacist/medicines` | Kho thuốc |
| GET | `/api/pharmacist/prescriptions/pending` | Đơn chờ cấp |
| PUT | `/api/pharmacist/prescriptions/{id}/dispense` | Cấp phát thuốc |

### Emergency
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/emergency` | Danh sách cấp cứu |
| POST | `/api/emergency/process` | Xử lý cấp cứu |

---

## 🗄️ Database Schema

### Entity Relationship Diagram

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│    User     │────<│  UserRole   │>────│    Role     │
└─────────────┘     └─────────────┘     └─────────────┘
      │                                       │
      │                                       │
      ├───────────────┬───────────────────────┘
      │               │
      ▼               ▼
┌─────────────┐ ┌─────────────┐ ┌─────────────┐
│   Doctor    │ │   Patient   │ │  Receptionist │
└─────────────┘ └─────────────┘ └─────────────┘
      │               │
      │               │
      ▼               ▼
┌─────────────┐ ┌─────────────┐
│ Appointment │ │ Appointment │
└─────────────┘ └─────────────┘
      │
      ▼
┌─────────────┐
│MedicalRecord│
└─────────────┘
      │
      ▼
┌─────────────┐     ┌─────────────┐
│ Prescription│────<│PrescriptionDetail│
└─────────────┘     └─────────────┘
                          │
                          ▼
                   ┌─────────────┐
                   │   Medicine  │
                   └─────────────┘
```

### Các bảng chính

| Bảng | Mô tả |
|------|-------|
| `user` | Tài khoản người dùng |
| `role` | Vai trò (ADMIN, DOCTOR, PATIENT...) |
| `user_role` | Liên kết user - role |
| `permission` | Quyền hạn |
| `role_permission` | Liên kết role - permission |
| `doctor` | Thông tin bác sĩ |
| `patient` | Thông tin bệnh nhân |
| `department` | Khoa phòng |
| `room` | Phòng khám |
| `appointment` | Lịch hẹn khám |
| `medical_record` | Hồ sơ bệnh án |
| `prescription` | Đơn thuốc |
| `prescription_detail` | Chi tiết đơn thuốc |
| `medicine` | Thuốc trong kho |
| `payment` | Thanh toán |
| `emergency_request` | Yêu cầu cấp cứu |
| `notification` | Thông báo |
| `doctor_schedule` | Lịch làm việc bác sĩ |

---

## 🔒 RBAC - Phân quyền

### 6 Vai trò chính

| Vai trò | Mã | Quyền hạn |
|---------|----|-----------|
| Quản trị viên | ADMIN | Toàn quyền hệ thống |
| Bác sĩ | DOCTOR | Khám bệnh, kê đơn |
| Y tá | NURSE | Hỗ trợ khám |
| Bệnh nhân | PATIENT | Đặt lịch, xem kết quả |
| Lễ tân | RECEPTIONIST | Tiếp nhận, quản lý hàng đợi |
| Dược sĩ | PHARMACIST | Quản lý kho thuốc, cấp thuốc |

### Dữ liệu mẫu được khởi tạo

- ✅ 6 vai trò
- ✅ 56 quyền hạn
- ✅ 1 Admin
- ✅ 10 Bác sĩ
- ✅ 30 Bệnh nhân
- ✅ 2 Lễ tân
- ✅ 2 Dược sĩ
- ✅ 10 Khoa
- ✅ 8 Phòng khám
- ✅ 50 Lịch hẹn
- ✅ 10 Loại thuốc
- ✅ 5 Loại bảo hiểm

---

## ⚙️ Cấu hình

### Backend (application.properties)
```properties
server.port=5000

# Database
spring.datasource.url=jdbc:h2:mem:qlbv
spring.datasource.driverClassName=org.h2.Driver
spring.datasource.username=sa
spring.datasource.password=

# JWT
jwt.secret=QuanLyBenhVienSecretKey2024VeryLongKeyForJWTTokenGeneration12345
jwt.expiration=86400000

# CORS
cors.allowed-origins=http://localhost:5001

# H2 Console
spring.h2.console.enabled=true
spring.h2.console.path=/h2-console
```

### Frontend (appsettings.json)
```json
{
  "ApiBaseUrl": "http://localhost:5000"
}
```

---

## 🐛 Xử lý sự cố

### Lỗi CORS
Đảm bảo backend cho phép origin `http://localhost:5001` trong CORS config.

### Lỗi 401/403
Kiểm tra JWT token còn hạn và được gửi đúng trong header.

### Database không load dữ liệu
Khởi động lại backend, dữ liệu sẽ được tự động seed.

---

## 📝 License

Đồ án môn học - Hệ thống Quản lý Bệnh viện

---

## 👨‍💻 Tác giả

MedCare Hospital Development Team

---

**Nếu cần hỗ trợ, vui lòng liên hệ qua GitHub Issues!**
