package com.hospital.qlbv.config;

import com.hospital.qlbv.entity.*;
import com.hospital.qlbv.repository.*;
import org.springframework.boot.CommandLineRunner;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Component;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.Random;

@Component
public class DataInitializer implements CommandLineRunner {

    private final RoleRepository roleRepository;
    private final UserRepository userRepository;
    private final UserRoleRepository userRoleRepository;
    private final DepartmentRepository departmentRepository;
    private final RoomRepository roomRepository;
    private final DoctorRepository doctorRepository;
    private final PatientRepository patientRepository;
    private final InsuranceRepository insuranceRepository;
    private final MedicineRepository medicineRepository;
    private final AppointmentRepository appointmentRepository;
    private final PermissionRepository permissionRepository;
    private final RolePermissionRepository rolePermissionRepository;
    private final PasswordEncoder passwordEncoder;

    private final Random random = new Random();

    public DataInitializer(RoleRepository roleRepository, UserRepository userRepository,
                         UserRoleRepository userRoleRepository, DepartmentRepository departmentRepository,
                         RoomRepository roomRepository, DoctorRepository doctorRepository,
                         PatientRepository patientRepository, InsuranceRepository insuranceRepository,
                         MedicineRepository medicineRepository, AppointmentRepository appointmentRepository,
                         PermissionRepository permissionRepository,
                         RolePermissionRepository rolePermissionRepository,
                         PasswordEncoder passwordEncoder) {
        this.roleRepository = roleRepository;
        this.userRepository = userRepository;
        this.userRoleRepository = userRoleRepository;
        this.departmentRepository = departmentRepository;
        this.roomRepository = roomRepository;
        this.doctorRepository = doctorRepository;
        this.patientRepository = patientRepository;
        this.insuranceRepository = insuranceRepository;
        this.medicineRepository = medicineRepository;
        this.appointmentRepository = appointmentRepository;
        this.permissionRepository = permissionRepository;
        this.rolePermissionRepository = rolePermissionRepository;
        this.passwordEncoder = passwordEncoder;
    }

    @Override
    public void run(String... args) {
        if (userRepository.count() > 0) {
            return; // Already initialized
        }

        String now = LocalDateTime.now().format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss"));

        // ==================== PERMISSIONS ====================
        // Permission format: RESOURCE_ACTION (e.g., PATIENT_VIEW, DOCTOR_CREATE)
        
        // PATIENT permissions
        Permission pViewOwn = permissionRepository.save(Permission.builder().permissionName("PATIENT_VIEW_OWN").description("Xem thông tin cá nhân").resource("PATIENT").action("VIEW_OWN").build());
        Permission pEditOwn = permissionRepository.save(Permission.builder().permissionName("PATIENT_EDIT_OWN").description("Sửa thông tin cá nhân").resource("PATIENT").action("EDIT_OWN").build());
        Permission pBookAppointment = permissionRepository.save(Permission.builder().permissionName("PATIENT_BOOK_APPOINTMENT").description("Đặt lịch khám").resource("APPOINTMENT").action("BOOK").build());
        Permission pCancelAppointment = permissionRepository.save(Permission.builder().permissionName("PATIENT_CANCEL_OWN").description("Hủy lịch khám của mình").resource("APPOINTMENT").action("CANCEL_OWN").build());
        Permission pViewRecord = permissionRepository.save(Permission.builder().permissionName("PATIENT_VIEW_RECORD").description("Xem bệnh án").resource("MEDICAL_RECORD").action("VIEW_OWN").build());
        Permission pViewPrescription = permissionRepository.save(Permission.builder().permissionName("PATIENT_VIEW_PRESCRIPTION").description("Xem đơn thuốc").resource("PRESCRIPTION").action("VIEW_OWN").build());
        Permission pViewPayment = permissionRepository.save(Permission.builder().permissionName("PATIENT_VIEW_PAYMENT").description("Xem hóa đơn").resource("PAYMENT").action("VIEW_OWN").build());
        Permission pPay = permissionRepository.save(Permission.builder().permissionName("PATIENT_PAY").description("Thanh toán").resource("PAYMENT").action("PAY").build());

        // DOCTOR permissions
        Permission dViewSchedule = permissionRepository.save(Permission.builder().permissionName("DOCTOR_VIEW_SCHEDULE").description("Xem lịch làm việc").resource("SCHEDULE").action("VIEW").build());
        Permission dViewPatients = permissionRepository.save(Permission.builder().permissionName("DOCTOR_VIEW_PATIENTS").description("Xem danh sách bệnh nhân").resource("PATIENT").action("VIEW").build());
        Permission dViewMedicalRecord = permissionRepository.save(Permission.builder().permissionName("DOCTOR_VIEW_RECORD").description("Xem bệnh án").resource("MEDICAL_RECORD").action("VIEW").build());
        Permission dCreateRecord = permissionRepository.save(Permission.builder().permissionName("DOCTOR_CREATE_RECORD").description("Tạo bệnh án").resource("MEDICAL_RECORD").action("CREATE").build());
        Permission dEditRecord = permissionRepository.save(Permission.builder().permissionName("DOCTOR_EDIT_RECORD").description("Sửa bệnh án").resource("MEDICAL_RECORD").action("EDIT").build());
        Permission dCreatePrescription = permissionRepository.save(Permission.builder().permissionName("DOCTOR_CREATE_PRESCRIPTION").description("Kê đơn thuốc").resource("PRESCRIPTION").action("CREATE").build());
        Permission dCreateTest = permissionRepository.save(Permission.builder().permissionName("DOCTOR_CREATE_TEST").description("Chỉ định xét nghiệm").resource("TEST_REQUEST").action("CREATE").build());
        Permission dViewAppointments = permissionRepository.save(Permission.builder().permissionName("DOCTOR_VIEW_APPOINTMENTS").description("Xem lịch hẹn").resource("APPOINTMENT").action("VIEW").build());
        Permission dUpdateAppointment = permissionRepository.save(Permission.builder().permissionName("DOCTOR_UPDATE_APPOINTMENT").description("Cập nhật lịch hẹn").resource("APPOINTMENT").action("UPDATE").build());

        // RECEPTIONIST permissions
        Permission rViewAllPatients = permissionRepository.save(Permission.builder().permissionName("RECEPTIONIST_VIEW_PATIENTS").description("Xem danh sách bệnh nhân").resource("PATIENT").action("VIEW_ALL").build());
        Permission rCreatePatient = permissionRepository.save(Permission.builder().permissionName("RECEPTIONIST_CREATE_PATIENT").description("Tạo hồ sơ bệnh nhân").resource("PATIENT").action("CREATE").build());
        Permission rEditPatient = permissionRepository.save(Permission.builder().permissionName("RECEPTIONIST_EDIT_PATIENT").description("Sửa hồ sơ bệnh nhân").resource("PATIENT").action("EDIT").build());
        Permission rViewAppointments = permissionRepository.save(Permission.builder().permissionName("RECEPTIONIST_VIEW_APPOINTMENTS").description("Xem lịch hẹn").resource("APPOINTMENT").action("VIEW_ALL").build());
        Permission rCreateAppointment = permissionRepository.save(Permission.builder().permissionName("RECEPTIONIST_CREATE_APPOINTMENT").description("Tạo lịch hẹn").resource("APPOINTMENT").action("CREATE").build());
        Permission rEditAppointment = permissionRepository.save(Permission.builder().permissionName("RECEPTIONIST_EDIT_APPOINTMENT").description("Sửa lịch hẹn").resource("APPOINTMENT").action("EDIT").build());
        Permission rCancelAppointment = permissionRepository.save(Permission.builder().permissionName("RECEPTIONIST_CANCEL_APPOINTMENT").description("Hủy lịch hẹn").resource("APPOINTMENT").action("CANCEL").build());
        Permission rCheckIn = permissionRepository.save(Permission.builder().permissionName("RECEPTIONIST_CHECKIN").description("Check-in bệnh nhân").resource("APPOINTMENT").action("CHECKIN").build());
        Permission rManageQueue = permissionRepository.save(Permission.builder().permissionName("RECEPTIONIST_MANAGE_QUEUE").description("Quản lý hàng đợi").resource("QUEUE").action("MANAGE").build());
        Permission rCreatePayment = permissionRepository.save(Permission.builder().permissionName("RECEPTIONIST_CREATE_PAYMENT").description("Tạo hóa đơn").resource("PAYMENT").action("CREATE").build());
        Permission rConfirmPayment = permissionRepository.save(Permission.builder().permissionName("RECEPTIONIST_CONFIRM_PAYMENT").description("Xác nhận thanh toán").resource("PAYMENT").action("CONFIRM").build());
        Permission rCreateEmergency = permissionRepository.save(Permission.builder().permissionName("RECEPTIONIST_CREATE_EMERGENCY").description("Tạo ca cấp cứu").resource("EMERGENCY").action("CREATE").build());

        // PHARMACIST permissions
        Permission phViewMedicines = permissionRepository.save(Permission.builder().permissionName("PHARMACIST_VIEW_MEDICINES").description("Xem danh sách thuốc").resource("MEDICINE").action("VIEW").build());
        Permission phCreateMedicine = permissionRepository.save(Permission.builder().permissionName("PHARMACIST_CREATE_MEDICINE").description("Thêm thuốc").resource("MEDICINE").action("CREATE").build());
        Permission phEditMedicine = permissionRepository.save(Permission.builder().permissionName("PHARMACIST_EDIT_MEDICINE").description("Sửa thông tin thuốc").resource("MEDICINE").action("EDIT").build());
        Permission phDeleteMedicine = permissionRepository.save(Permission.builder().permissionName("PHARMACIST_DELETE_MEDICINE").description("Xóa thuốc").resource("MEDICINE").action("DELETE").build());
        Permission phViewPrescriptions = permissionRepository.save(Permission.builder().permissionName("PHARMACIST_VIEW_PRESCRIPTIONS").description("Xem đơn thuốc").resource("PRESCRIPTION").action("VIEW_ALL").build());
        Permission phDispense = permissionRepository.save(Permission.builder().permissionName("PHARMACIST_DISPENSE").description("Cấp phát thuốc").resource("PRESCRIPTION").action("DISPENSE").build());
        Permission phViewReports = permissionRepository.save(Permission.builder().permissionName("PHARMACIST_VIEW_REPORTS").description("Xem báo cáo kho").resource("REPORT").action("VIEW").build());

        // ADMIN permissions
        Permission aViewDashboard = permissionRepository.save(Permission.builder().permissionName("ADMIN_VIEW_DASHBOARD").description("Xem dashboard").resource("DASHBOARD").action("VIEW").build());
        Permission aViewUsers = permissionRepository.save(Permission.builder().permissionName("ADMIN_VIEW_USERS").description("Xem danh sách users").resource("USER").action("VIEW_ALL").build());
        Permission aCreateUser = permissionRepository.save(Permission.builder().permissionName("ADMIN_CREATE_USER").description("Tạo user").resource("USER").action("CREATE").build());
        Permission aEditUser = permissionRepository.save(Permission.builder().permissionName("ADMIN_EDIT_USER").description("Sửa user").resource("USER").action("EDIT").build());
        Permission aDeleteUser = permissionRepository.save(Permission.builder().permissionName("ADMIN_DELETE_USER").description("Xóa user").resource("USER").action("DELETE").build());
        Permission aViewDoctors = permissionRepository.save(Permission.builder().permissionName("ADMIN_VIEW_DOCTORS").description("Xem danh sách bác sĩ").resource("DOCTOR").action("VIEW_ALL").build());
        Permission aCreateDoctor = permissionRepository.save(Permission.builder().permissionName("ADMIN_CREATE_DOCTOR").description("Thêm bác sĩ").resource("DOCTOR").action("CREATE").build());
        Permission aEditDoctor = permissionRepository.save(Permission.builder().permissionName("ADMIN_EDIT_DOCTOR").description("Sửa bác sĩ").resource("DOCTOR").action("EDIT").build());
        Permission aDeleteDoctor = permissionRepository.save(Permission.builder().permissionName("ADMIN_DELETE_DOCTOR").description("Xóa bác sĩ").resource("DOCTOR").action("DELETE").build());
        Permission aViewDepartments = permissionRepository.save(Permission.builder().permissionName("ADMIN_VIEW_DEPARTMENTS").description("Xem khoa").resource("DEPARTMENT").action("VIEW_ALL").build());
        Permission aManageDepartment = permissionRepository.save(Permission.builder().permissionName("ADMIN_MANAGE_DEPARTMENT").description("Quản lý khoa").resource("DEPARTMENT").action("MANAGE").build());
        Permission aViewMedicines = permissionRepository.save(Permission.builder().permissionName("ADMIN_VIEW_MEDICINES").description("Xem thuốc").resource("MEDICINE").action("VIEW_ALL").build());
        Permission aManageMedicines = permissionRepository.save(Permission.builder().permissionName("ADMIN_MANAGE_MEDICINES").description("Quản lý thuốc").resource("MEDICINE").action("MANAGE").build());
        Permission aViewAppointments = permissionRepository.save(Permission.builder().permissionName("ADMIN_VIEW_APPOINTMENTS").description("Xem lịch hẹn").resource("APPOINTMENT").action("VIEW_ALL").build());
        Permission aManageAppointments = permissionRepository.save(Permission.builder().permissionName("ADMIN_MANAGE_APPOINTMENTS").description("Quản lý lịch hẹn").resource("APPOINTMENT").action("MANAGE").build());
        Permission aViewPayments = permissionRepository.save(Permission.builder().permissionName("ADMIN_VIEW_PAYMENTS").description("Xem thanh toán").resource("PAYMENT").action("VIEW_ALL").build());
        Permission aViewReports = permissionRepository.save(Permission.builder().permissionName("ADMIN_VIEW_REPORTS").description("Xem báo cáo").resource("REPORT").action("VIEW_ALL").build());
        Permission aManageServices = permissionRepository.save(Permission.builder().permissionName("ADMIN_MANAGE_SERVICES").description("Quản lý dịch vụ").resource("SERVICE").action("MANAGE").build());
        Permission aViewEmergency = permissionRepository.save(Permission.builder().permissionName("ADMIN_VIEW_EMERGENCY").description("Xem cấp cứu").resource("EMERGENCY").action("VIEW_ALL").build());
        Permission aManageEmergency = permissionRepository.save(Permission.builder().permissionName("ADMIN_MANAGE_EMERGENCY").description("Quản lý cấp cứu").resource("EMERGENCY").action("MANAGE").build());

        // ==================== ROLES ====================
        Role adminRole = roleRepository.save(Role.builder().roleName("ADMIN").description("Quản trị hệ thống").build());
        Role doctorRole = roleRepository.save(Role.builder().roleName("DOCTOR").description("Bác sĩ").build());
        Role nurseRole = roleRepository.save(Role.builder().roleName("NURSE").description("Y tá / Điều dưỡng").build());
        Role patientRole = roleRepository.save(Role.builder().roleName("PATIENT").description("Bệnh nhân").build());
        Role receptionistRole = roleRepository.save(Role.builder().roleName("RECEPTIONIST").description("Lễ tân").build());
        Role pharmacistRole = roleRepository.save(Role.builder().roleName("PHARMACIST").description("Dược sĩ").build());

        // ==================== ROLE-PERMISSION MAPPINGS ====================
        // ADMIN - full access
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aViewDashboard.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aViewUsers.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aCreateUser.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aEditUser.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aDeleteUser.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aViewDoctors.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aCreateDoctor.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aEditDoctor.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aDeleteDoctor.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aViewDepartments.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aManageDepartment.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aViewMedicines.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aManageMedicines.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aViewAppointments.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aManageAppointments.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aViewPayments.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aViewReports.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aManageServices.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aViewEmergency.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(aManageEmergency.getPermissionId()).build());
        
        // Also give ADMIN some staff-like permissions
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(rViewAllPatients.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(adminRole.getRoleId()).permissionId(rCreateAppointment.getPermissionId()).build());

        // DOCTOR permissions
        rolePermissionRepository.save(RolePermission.builder().roleId(doctorRole.getRoleId()).permissionId(dViewSchedule.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(doctorRole.getRoleId()).permissionId(dViewPatients.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(doctorRole.getRoleId()).permissionId(dViewMedicalRecord.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(doctorRole.getRoleId()).permissionId(dCreateRecord.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(doctorRole.getRoleId()).permissionId(dEditRecord.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(doctorRole.getRoleId()).permissionId(dCreatePrescription.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(doctorRole.getRoleId()).permissionId(dCreateTest.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(doctorRole.getRoleId()).permissionId(dViewAppointments.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(doctorRole.getRoleId()).permissionId(dUpdateAppointment.getPermissionId()).build());

        // PATIENT permissions
        rolePermissionRepository.save(RolePermission.builder().roleId(patientRole.getRoleId()).permissionId(pViewOwn.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(patientRole.getRoleId()).permissionId(pEditOwn.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(patientRole.getRoleId()).permissionId(pBookAppointment.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(patientRole.getRoleId()).permissionId(pCancelAppointment.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(patientRole.getRoleId()).permissionId(pViewRecord.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(patientRole.getRoleId()).permissionId(pViewPrescription.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(patientRole.getRoleId()).permissionId(pViewPayment.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(patientRole.getRoleId()).permissionId(pPay.getPermissionId()).build());

        // RECEPTIONIST permissions
        rolePermissionRepository.save(RolePermission.builder().roleId(receptionistRole.getRoleId()).permissionId(rViewAllPatients.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(receptionistRole.getRoleId()).permissionId(rCreatePatient.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(receptionistRole.getRoleId()).permissionId(rEditPatient.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(receptionistRole.getRoleId()).permissionId(rViewAppointments.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(receptionistRole.getRoleId()).permissionId(rCreateAppointment.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(receptionistRole.getRoleId()).permissionId(rEditAppointment.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(receptionistRole.getRoleId()).permissionId(rCancelAppointment.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(receptionistRole.getRoleId()).permissionId(rCheckIn.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(receptionistRole.getRoleId()).permissionId(rManageQueue.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(receptionistRole.getRoleId()).permissionId(rCreatePayment.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(receptionistRole.getRoleId()).permissionId(rConfirmPayment.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(receptionistRole.getRoleId()).permissionId(rCreateEmergency.getPermissionId()).build());

        // PHARMACIST permissions
        rolePermissionRepository.save(RolePermission.builder().roleId(pharmacistRole.getRoleId()).permissionId(phViewMedicines.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(pharmacistRole.getRoleId()).permissionId(phCreateMedicine.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(pharmacistRole.getRoleId()).permissionId(phEditMedicine.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(pharmacistRole.getRoleId()).permissionId(phDeleteMedicine.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(pharmacistRole.getRoleId()).permissionId(phViewPrescriptions.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(pharmacistRole.getRoleId()).permissionId(phDispense.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(pharmacistRole.getRoleId()).permissionId(phViewReports.getPermissionId()).build());

        // NURSE permissions (limited)
        rolePermissionRepository.save(RolePermission.builder().roleId(nurseRole.getRoleId()).permissionId(dViewPatients.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(nurseRole.getRoleId()).permissionId(dViewMedicalRecord.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(nurseRole.getRoleId()).permissionId(dEditRecord.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(nurseRole.getRoleId()).permissionId(rCheckIn.getPermissionId()).build());
        rolePermissionRepository.save(RolePermission.builder().roleId(nurseRole.getRoleId()).permissionId(rViewAppointments.getPermissionId()).build());

        // Create Admin User
        User admin = User.builder()
                .username("admin")
                .passwordHash(passwordEncoder.encode("1"))
                .email("admin@hospital.com")
                .fullName("Quản trị viên")
                .phone("0901234567")
                .gender("Nam")
                .status("Hoạt động")
                .createdAt(now)
                .build();
        admin = userRepository.save(admin);
        userRoleRepository.save(UserRole.builder().userId(admin.getUserId()).roleId(adminRole.getRoleId()).build());

        // Create Receptionist Users
        User[] receptionists = new User[2];
        String[][] receptionistData = {
            {"receptionist1", "Nguyễn Thị Thu", "receptionist1@hospital.com", "0909888777", "Nữ"},
            {"receptionist2", "Trần Văn Nam", "receptionist2@hospital.com", "0909888778", "Nam"}
        };
        for (int i = 0; i < receptionistData.length; i++) {
            String[] r = receptionistData[i];
            User rec = User.builder()
                    .username(r[0])
                    .passwordHash(passwordEncoder.encode("123456"))
                    .email(r[2])
                    .fullName(r[1])
                    .phone(r[3])
                    .gender(r[4])
                    .status("Hoạt động")
                    .createdAt(now)
                    .build();
            receptionists[i] = userRepository.save(rec);
            userRoleRepository.save(UserRole.builder().userId(receptionists[i].getUserId()).roleId(receptionistRole.getRoleId()).build());
        }

        // Create Pharmacist Users
        User[] pharmacists = new User[2];
        String[][] pharmacistData = {
            {"pharmacist1", "Lê Thị Lan", "pharmacist1@hospital.com", "0909777666", "Nữ"},
            {"pharmacist2", "Phạm Văn Hùng", "pharmacist2@hospital.com", "0909777667", "Nam"}
        };
        for (int i = 0; i < pharmacistData.length; i++) {
            String[] p = pharmacistData[i];
            User phar = User.builder()
                    .username(p[0])
                    .passwordHash(passwordEncoder.encode("123456"))
                    .email(p[2])
                    .fullName(p[1])
                    .phone(p[3])
                    .gender(p[4])
                    .status("Hoạt động")
                    .createdAt(now)
                    .build();
            pharmacists[i] = userRepository.save(phar);
            userRoleRepository.save(UserRole.builder().userId(pharmacists[i].getUserId()).roleId(pharmacistRole.getRoleId()).build());
        }

        // Create Departments
        Department deptNoiTongHop = departmentRepository.save(Department.builder()
                .departmentName("Nội tổng hợp").description("Khám và điều trị các bệnh nội khoa").location("Tầng 2 - P201").status("Hoạt động").build());
        Department deptNgoaiTongHop = departmentRepository.save(Department.builder()
                .departmentName("Ngoại tổng hợp").description("Phẫu thuật và điều trị ngoại khoa").location("Tầng 3 - P301").status("Hoạt động").build());
        Department deptNhiKhoa = departmentRepository.save(Department.builder()
                .departmentName("Nhi khoa").description("Chăm sóc sức khỏe trẻ em").location("Tầng 4 - P401").status("Hoạt động").build());
        Department deptSanPhuKhoa = departmentRepository.save(Department.builder()
                .departmentName("Sản phụ khoa").description("Sản khoa và phụ khoa").location("Tầng 5 - P501").status("Hoạt động").build());
        Department deptTimMach = departmentRepository.save(Department.builder()
                .departmentName("Tim mạch").description("Tim mạch và can thiệp tim").location("Tầng 6 - P601").status("Hoạt động").build());
        Department deptCapCuu = departmentRepository.save(Department.builder()
                .departmentName("Cấp cứu").description("Cấp cứu 24/7").location("Tầng 1 - P101").status("Hoạt động").build());
        Department deptTaiMuiHong = departmentRepository.save(Department.builder()
                .departmentName("Tai Mũi Họng").description("Tai, Mũi, Họng").location("Tầng 2 - P202").status("Hoạt động").build());
        Department deptDaLieu = departmentRepository.save(Department.builder()
                .departmentName("Da liễu").description("Da liễu và hoa liễu").location("Tầng 2 - P203").status("Hoạt động").build());

        // Create Rooms
        Room room1 = roomRepository.save(Room.builder().roomNumber("201").departmentId(deptNoiTongHop.getDepartmentId()).roomType("Phòng khám").status("Hoạt động").build());
        Room room2 = roomRepository.save(Room.builder().roomNumber("202").departmentId(deptNoiTongHop.getDepartmentId()).roomType("Phòng khám").status("Hoạt động").build());
        Room room3 = roomRepository.save(Room.builder().roomNumber("301").departmentId(deptNgoaiTongHop.getDepartmentId()).roomType("Phòng mổ").status("Hoạt động").build());
        Room room4 = roomRepository.save(Room.builder().roomNumber("401").departmentId(deptNhiKhoa.getDepartmentId()).roomType("Phòng khám").status("Hoạt động").build());
        Room room5 = roomRepository.save(Room.builder().roomNumber("501").departmentId(deptSanPhuKhoa.getDepartmentId()).roomType("Phòng khám").status("Hoạt động").build());
        Room room6 = roomRepository.save(Room.builder().roomNumber("601").departmentId(deptTimMach.getDepartmentId()).roomType("Phòng khám").status("Hoạt động").build());
        Room room7 = roomRepository.save(Room.builder().roomNumber("203").departmentId(deptTaiMuiHong.getDepartmentId()).roomType("Phòng khám").status("Hoạt động").build());
        Room room8 = roomRepository.save(Room.builder().roomNumber("204").departmentId(deptDaLieu.getDepartmentId()).roomType("Phòng khám").status("Hoạt động").build());

        // ==================== 10 BÁC SĨ ====================
        Department[] departments = {deptNoiTongHop, deptNgoaiTongHop, deptNhiKhoa, deptSanPhuKhoa, deptTimMach, deptTaiMuiHong, deptDaLieu};
        Room[] rooms = {room1, room2, room3, room4, room5, room6, room7, room8};

        String[][] doctorData = {
            {"dr.nguyenvana", "Bs. Nguyễn Văn A", "dr.nguyenvana@hospital.com", "0902345678", "Nam", "Nội tổng hợp", "15 năm kinh nghiệm", "Đại học Y dược TP.HCM", "Thứ 2 - Thứ 6: 8:00 - 17:00", "250000"},
            {"dr.tranthib", "Bs. Trần Thị B", "dr.tranthib@hospital.com", "0902345679", "Nữ", "Ngoại tổng hợp", "12 năm kinh nghiệm", "Đại học Y Hà Nội", "Thứ 2 - Thứ 6: 8:00 - 17:00", "300000"},
            {"dr.levanc", "Bs. Lê Văn C", "dr.levancc@hospital.com", "0902345680", "Nam", "Nhi khoa", "10 năm kinh nghiệm", "Đại học Y TP.HCM", "Thứ 2 - Thứ 6: 7:30 - 16:30", "200000"},
            {"dr.phamthid", "Bs. Phạm Thị D", "dr.phamthid@hospital.com", "0902345681", "Nữ", "Sản phụ khoa", "18 năm kinh nghiệm", "Đại học Y dược Hà Nội", "Thứ 2 - Thứ 6: 8:00 - 17:00", "350000"},
            {"dr.hoangvane", "Bs. Hoàng Văn E", "dr.hoangvane@hospital.com", "0902345682", "Nam", "Tim mạch", "20 năm kinh nghiệm", "Đại học Y Hà Nội", "Thứ 2 - Thứ 7: 8:00 - 18:00", "400000"},
            {"dr.nguyenthif", "Bs. Nguyễn Thị F", "dr.nguyenthif@hospital.com", "0902345683", "Nữ", "Nội tổng hợp", "8 năm kinh nghiệm", "Đại học Y Thái Bình", "Thứ 2 - Thứ 6: 8:00 - 17:00", "220000"},
            {"dr.tranvanh", "Bs. Trần Văn H", "dr.tranvanh@hospital.com", "0902345684", "Nam", "Tai Mũi Họng", "14 năm kinh nghiệm", "Đại học Y dược TP.HCM", "Thứ 2 - Thứ 6: 8:00 - 17:00", "230000"},
            {"dr.levani", "Bs. Lê Văn I", "dr.levani@hospital.com", "0902345685", "Nam", "Da liễu", "11 năm kinh nghiệm", "Đại học Y Hà Nội", "Thứ 2 - Thứ 6: 8:00 - 17:00", "200000"},
            {"dr.phamthik", "Bs. Phạm Thị K", "dr.phamthik@hospital.com", "0902345686", "Nữ", "Nhi khoa", "9 năm kinh nghiệm", "Đại học Y dược Đà Nẵng", "Thứ 2 - Thứ 6: 7:30 - 16:30", "180000"},
            {"dr.hoangthil", "Bs. Hoàng Thị L", "dr.hoangthil@hospital.com", "0902345687", "Nữ", "Sản phụ khoa", "16 năm kinh nghiệm", "Đại học Y TP.HCM", "Thứ 2 - Thứ 6: 8:00 - 17:00", "320000"}
        };

        Doctor[] doctors = new Doctor[10];
        for (int i = 0; i < doctorData.length; i++) {
            String[] d = doctorData[i];
            User doctorUser = User.builder()
                    .username(d[0])
                    .passwordHash(passwordEncoder.encode("123456"))
                    .email(d[2])
                    .fullName(d[1])
                    .phone(d[3])
                    .gender(d[4])
                    .status("Hoạt động")
                    .createdAt(now)
                    .build();
            doctorUser = userRepository.save(doctorUser);
            userRoleRepository.save(UserRole.builder().userId(doctorUser.getUserId()).roleId(doctorRole.getRoleId()).build());

            int deptIndex = i % departments.length;
            int roomIndex = i % rooms.length;
            
            Doctor doctor = Doctor.builder()
                    .userId(doctorUser.getUserId())
                    .departmentId(departments[deptIndex].getDepartmentId())
                    .roomId(rooms[roomIndex].getRoomId())
                    .specialty(d[5])
                    .experience(d[6])
                    .education(d[7])
                    .workingSchedule(d[8])
                    .consultationFee(new BigDecimal(d[9]))
                    .status("Hoạt động")
                    .build();
            doctors[i] = doctorRepository.save(doctor);
        }

        // Create Insurance types
        String[][] insuranceData = {
            {"Bảo hiểm Y tế Quốc gia", "BHYT", "90000000"},
            {"Bảo hiểm Bảo Việt", "BVHI", "50000000"},
            {"Bảo hiểm Prudential", "PRU", "100000000"},
            {"Bảo hiểm Manulife", "MANU", "80000000"},
            {"Bảo hiểm AIA", "AIA", "70000000"}
        };

        // ==================== 30 BỆNH NHÂN ====================
        String[][] patientData = {
            {"Nguyễn Minh Tuấn", "minhtuan@email.com", "0911234567", "Nam", "1985-03-15", "123 Nguyễn Trãi, Q1, TP.HCM", "A", "170", "75"},
            {"Trần Hồng Anh", "honganh@email.com", "0911234568", "Nữ", "1990-07-22", "45 Lê Lợi, Q3, TP.HCM", "B", "158", "52"},
            {"Lê Quốc Minh", "quocminh@email.com", "0911234569", "Nam", "1978-11-08", "78 Hai Bà Trưng, Q1, HN", "O", "175", "80"},
            {"Phạm Thị Lan", "thilan@email.com", "0911234570", "Nữ", "1995-05-30", "56 Điện Biên Phủ, Q3, TP.HCM", "AB", "155", "48"},
            {"Hoàng Đức Anh", "ducanh@email.com", "0911234571", "Nam", "1988-09-12", "89 Pasteur, Q1, TP.HCM", "A", "172", "68"},
            {"Vũ Thị Mai", "thimai@email.com", "0911234572", "Nữ", "1992-01-25", "34 Nguyễn Huệ, Q1, TP.HCM", "O", "160", "55"},
            {"Đặng Minh Khoa", "minhkhoa@email.com", "0911234573", "Nam", "1980-12-03", "67 Lý Thường Kiệt, Q10, TP.HCM", "B", "168", "72"},
            {"Bùi Thị Hương", "thihuong@email.com", "0911234574", "Nữ", "1987-04-18", "91 Trần Hưng Đạo, Q5, TP.HCM", "A", "162", "58"},
            {"Trịnh Quốc Trung", "quoctrung@email.com", "0911234575", "Nam", "1975-08-07", "23 Đồng Khởi, Q1, TP.HCM", "O", "177", "85"},
            {"Ngô Thị Thu", "thithu@email.com", "0911234576", "Nữ", "1998-02-14", "15 Võ Văn Tần, Q3, TP.HCM", "AB", "157", "50"},
            {"Phan Văn Hùng", "vanhung@email.com", "0911234577", "Nam", "1983-06-20", "42 Trương Định, Q3, TP.HCM", "A", "171", "78"},
            {"Đỗ Thị Phương", "thiphuong@email.com", "0911234578", "Nữ", "1991-10-05", "73 Phạm Ngũ Lão, Q1, TP.HCM", "B", "159", "54"},
            {"Lý Đình Nam", "dinhnham@email.com", "0911234579", "Nam", "1972-03-28", "88 Lê Lai, Q3, TP.HCM", "O", "174", "82"},
            {"Trần Văn Đức", "vanduc@email.com", "0911234580", "Nam", "1994-07-11", "62 Nguyễn Thị Minh Khai, Q3, TP.HCM", "A", "169", "65"},
            {"Lê Thị Hồng", "thihong@email.com", "0911234581", "Nữ", "1986-11-30", "37 Cách Mạng Tháng 8, Q10, TP.HCM", "AB", "161", "60"},
            {"Nguyễn Hữu Thắng", "huuthang@email.com", "0911234582", "Nam", "1979-05-16", "54 Trần Não, Q2, TP.HCM", "B", "173", "76"},
            {"Trịnh Thị Lan", "thilan2@email.com", "0911234583", "Nữ", "1993-09-23", "81 Bà Huyện Thanh Quan, Q3, TP.HCM", "O", "158", "51"},
            {"Vũ Đức Minh", "ducminh@email.com", "0911234584", "Nam", "1989-01-08", "29 Trần Cao Vân, Q3, TP.HCM", "A", "176", "79"},
            {"Đặng Thị Mai", "thimai2@email.com", "0911234585", "Nữ", "1996-04-25", "48 Vạn Hạnh, Q10, TP.HCM", "AB", "156", "49"},
            {"Bùi Văn Tuấn", "vantuan@email.com", "0911234586", "Nam", "1984-08-02", "66 Đại lộ Thăng Long, Q2, HN", "B", "170", "74"},
            {"Lý Thị Hà", "thiha@email.com", "0911234587", "Nữ", "1997-12-19", "52 Lê Quý Đôn, Q3, TP.HCM", "O", "163", "57"},
            {"Hoàng Đức Long", "duclong@email.com", "0911234588", "Nam", "1974-06-14", "71 Trần Phú, Q5, TP.HCM", "A", "175", "83"},
            {"Ngô Thị Kim", "thikim@email.com", "0911234589", "Nữ", "1999-02-28", "93 Nguyễn Tri Phương, Q10, TP.HCM", "B", "159", "53"},
            {"Trần Đình Khôi", "dinhkhoi@email.com", "0911234590", "Nam", "1982-10-11", "84 Hoàng Sa, Q3, TP.HCM", "AB", "171", "77"},
            {"Phạm Văn Minh", "vanminh@email.com", "0911234591", "Nam", "1990-07-04", "19 Hùng Vương, Q5, TP.HCM", "O", "168", "70"},
            {"Đỗ Thị Hạnh", "thihanh@email.com", "0911234592", "Nữ", "1988-03-22", "61 Trần Bình Trọng, Q5, TP.HCM", "A", "160", "56"},
            {"Lê Hữu Phúc", "huuphucc@email.com", "0911234593", "Nam", "1976-09-17", "77 Nguyễn Văn Cừ, Q1, HN", "B", "178", "88"},
            {"Trịnh Thị Thanh", "thithanh@email.com", "0911234594", "Nữ", "1994-05-09", "33 Phan Chu Trinh, Q10, TP.HCM", "O", "157", "52"},
            {"Vũ Quang Hùng", "quanghung@email.com", "0911234595", "Nam", "1981-11-26", "85 Ngô Gia Tự, Q10, TP.HCM", "AB", "172", "75"},
            {"Bùi Thị Minh", "thiminh@email.com", "0911234596", "Nữ", "1993-08-13", "47 Đường 3/2, Q10, TP.HCM", "A", "161", "58"}
        };

        for (int i = 0; i < patientData.length; i++) {
            String[] p = patientData[i];
            
            String[] insData = insuranceData[random.nextInt(insuranceData.length)];
            String policyNumber = insData[1] + String.format("%06d", i + 1);
            int expiryYear = LocalDate.now().getYear() + random.nextInt(3) + 1;
            
            Insurance insurance = insuranceRepository.save(Insurance.builder()
                    .insuranceProvider(insData[0])
                    .policyNumber(policyNumber)
                    .expiryDate(expiryYear + "-12-31")
                    .coverageAmount(Double.parseDouble(insData[2]))
                    .build());

            String patientUsername = "patient" + (i + 1);
            User patientUser = User.builder()
                    .username(patientUsername)
                    .passwordHash(passwordEncoder.encode("123456"))
                    .email(p[1])
                    .fullName(p[0])
                    .phone(p[2])
                    .gender(p[3])
                    .address(p[5])
                    .dateOfBirth(p[4])
                    .status("Hoạt động")
                    .createdAt(now)
                    .build();
            patientUser = userRepository.save(patientUser);
            userRoleRepository.save(UserRole.builder().userId(patientUser.getUserId()).roleId(patientRole.getRoleId()).build());

            Patient patient = Patient.builder()
                    .userId(patientUser.getUserId())
                    .insuranceId(insurance.getInsuranceId())
                    .bloodType(p[6])
                    .height(new BigDecimal(p[7]))
                    .weight(new BigDecimal(p[8]))
                    .allergies(random.nextBoolean() ? "Dị ứng penicillin" : null)
                    .status("Hoạt động")
                    .createdAt(now)
                    .build();
            patientRepository.save(patient);
        }

        // ==================== TẠO LỊCH HẸN ====================
        String[] timeSlots = {"08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "14:00", "14:30", "15:00", "15:30", "16:00"};
        String[] appointmentTypes = {"Khám định kỳ", "Khám bệnh", "Tái khám", "Khám chuyên khoa"};
        String[] reasons = {
            "Đau bụng dữ dội", "Sốt cao không giảm", "Đau đầu liên tục", "Kiểm tra sức khỏe",
            "Tái khám theo yêu cầu", "Khám tim định kỳ", "Đau lưng", "Ho kéo dài",
            "Khó thở nhẹ", "Mệt mỏi kéo dài", "Chóng mặt", "Buồn nôn"
        };

        var allPatients = patientRepository.findAll();
        var allDoctors = doctorRepository.findAll();

        for (int i = 0; i < 50; i++) {
            Patient randomPatient = allPatients.get(random.nextInt(allPatients.size()));
            Doctor randomDoctor = allDoctors.get(random.nextInt(allDoctors.size()));
            
            int daysOffset = random.nextInt(60) - 20;
            LocalDate appointmentDate = LocalDate.now().plusDays(daysOffset);
            
            String status;
            if (daysOffset < 0) {
                status = "Hoàn thành";
            } else if (daysOffset == 0) {
                status = "Đã xác nhận";
            } else {
                status = random.nextBoolean() ? "Đã xác nhận" : "Chờ xác nhận";
            }

            String appointmentType = appointmentTypes[random.nextInt(appointmentTypes.length)];
            String reason = reasons[random.nextInt(reasons.length)];
            String timeSlot = timeSlots[random.nextInt(timeSlots.length)];

            appointmentRepository.save(Appointment.builder()
                    .patientId(randomPatient.getPatientId())
                    .doctorId(randomDoctor.getDoctorId())
                    .departmentId(randomDoctor.getDepartmentId())
                    .appointmentDate(appointmentDate.toString())
                    .timeSlot(timeSlot)
                    .reason(reason)
                    .appointmentType(appointmentType)
                    .status(status)
                    .build());
        }

        // Create Medicines
        medicineRepository.save(Medicine.builder()
                .medicineName("Paracetamol 500mg").genericName("Acetaminophen")
                .manufacturer("Stada").unit("Viên").price(new BigDecimal("500")).stock(10000)
                .dosageForm("Viên nén").status("Hoạt động").build());
        medicineRepository.save(Medicine.builder()
                .medicineName("Amoxicillin 500mg").genericName("Amoxicillin")
                .manufacturer("Domesco").unit("Viên").price(new BigDecimal("800")).stock(5000)
                .dosageForm("Viên nang").status("Hoạt động").build());
        medicineRepository.save(Medicine.builder()
                .medicineName("Ibuprofen 400mg").genericName("Ibuprofen")
                .manufacturer("Berlin").unit("Viên").price(new BigDecimal("1000")).stock(3000)
                .dosageForm("Viên nén").status("Hoạt động").build());
        medicineRepository.save(Medicine.builder()
                .medicineName("Vitamin C 500mg").genericName("Ascorbic acid")
                .manufacturer("Pharmacity").unit("Viên").price(new BigDecimal("300")).stock(15000)
                .dosageForm("Viên sủi").status("Hoạt động").build());
        medicineRepository.save(Medicine.builder()
                .medicineName("Omeprazole 20mg").genericName("Omeprazole")
                .manufacturer("Daewoong").unit("Viên").price(new BigDecimal("2000")).stock(2000)
                .dosageForm("Viên nang").status("Hoạt động").build());
        medicineRepository.save(Medicine.builder()
                .medicineName("Metformin 500mg").genericName("Metformin")
                .manufacturer("Domesco").unit("Viên").price(new BigDecimal("1500")).stock(3000)
                .dosageForm("Viên nén").status("Hoạt động").build());
        medicineRepository.save(Medicine.builder()
                .medicineName("Amlodipine 5mg").genericName("Amlodipine")
                .manufacturer("Stada").unit("Viên").price(new BigDecimal("800")).stock(4000)
                .dosageForm("Viên nén").status("Hoạt động").build());
        medicineRepository.save(Medicine.builder()
                .medicineName("Loratadine 10mg").genericName("Loratadine")
                .manufacturer("Berlin").unit("Viên").price(new BigDecimal("600")).stock(5000)
                .dosageForm("Viên nén").status("Hoạt động").build());
        medicineRepository.save(Medicine.builder()
                .medicineName("Diclofenac 50mg").genericName("Diclofenac")
                .manufacturer("Daewoong").unit("Viên").price(new BigDecimal("1200")).stock(2500)
                .dosageForm("Viên nén").status("Hoạt động").build());
        medicineRepository.save(Medicine.builder()
                .medicineName("Ciprofloxacin 500mg").genericName("Ciprofloxacin")
                .manufacturer("Pharmacity").unit("Viên").price(new BigDecimal("2500")).stock(2000)
                .dosageForm("Viên nang").status("Hoạt động").build());

        System.out.println("========================================");
        System.out.println("Data initialized with RBAC!");
        System.out.println("- Roles: 6 (ADMIN, DOCTOR, NURSE, PATIENT, RECEPTIONIST, PHARMACIST)");
        System.out.println("- Permissions: " + permissionRepository.count());
        System.out.println("- 10 Doctors");
        System.out.println("- 30 Patients");
        System.out.println("- 2 Receptionists");
        System.out.println("- 2 Pharmacists");
        System.out.println("- 50 Appointments");
        System.out.println("- 10 Departments, 8 Rooms");
        System.out.println("- 10 Medicines");
        System.out.println("========================================");
    }
}
