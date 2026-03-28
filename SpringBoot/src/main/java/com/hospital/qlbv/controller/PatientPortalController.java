package com.hospital.qlbv.controller;

import com.hospital.qlbv.dto.ApiResponse;
import com.hospital.qlbv.entity.*;
import com.hospital.qlbv.repository.*;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/patient")
public class PatientPortalController {

    private final UserRepository userRepository;
    private final PatientRepository patientRepository;
    private final DoctorRepository doctorRepository;
    private final DepartmentRepository departmentRepository;
    private final AppointmentRepository appointmentRepository;
    private final MedicalRecordRepository medicalRecordRepository;
    private final PrescriptionRepository prescriptionRepository;
    private final PaymentRepository paymentRepository;
    private final UserRoleRepository userRoleRepository;
    private final RoleRepository roleRepository;

    public PatientPortalController(UserRepository userRepository, PatientRepository patientRepository,
                                  DoctorRepository doctorRepository, DepartmentRepository departmentRepository,
                                  AppointmentRepository appointmentRepository,
                                  MedicalRecordRepository medicalRecordRepository,
                                  PrescriptionRepository prescriptionRepository,
                                  PaymentRepository paymentRepository,
                                  UserRoleRepository userRoleRepository, RoleRepository roleRepository) {
        this.userRepository = userRepository;
        this.patientRepository = patientRepository;
        this.doctorRepository = doctorRepository;
        this.departmentRepository = departmentRepository;
        this.appointmentRepository = appointmentRepository;
        this.medicalRecordRepository = medicalRecordRepository;
        this.prescriptionRepository = prescriptionRepository;
        this.paymentRepository = paymentRepository;
        this.userRoleRepository = userRoleRepository;
        this.roleRepository = roleRepository;
    }

    // Helper to get patient ID from authentication
    private Integer getPatientIdFromAuth(Authentication auth) {
        String username = auth.getName();
        User user = userRepository.findByUsername(username).orElse(null);
        if (user == null) return null;
        
        // Check if user is a patient
        boolean isPatient = user.getUserRoles().stream()
                .anyMatch(ur -> "PATIENT".equals(ur.getRole().getRoleName()));
        if (!isPatient) return null;
        
        return patientRepository.findByUserId(user.getUserId())
                .map(Patient::getPatientId)
                .orElse(null);
    }

    // GET /api/patient/profile - Get own profile
    @GetMapping("/profile")
    @PreAuthorize("hasAnyRole('PATIENT', 'ADMIN')")
    public ResponseEntity<?> getProfile(Authentication auth) {
        Integer patientId = getPatientIdFromAuth(auth);
        if (patientId == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy hồ sơ bệnh nhân"));
        }

        Patient patient = patientRepository.findById(patientId).orElse(null);
        if (patient == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy bệnh nhân"));
        }

        Map<String, Object> profile = new HashMap<>();
        profile.put("patientId", patient.getPatientId());
        profile.put("bloodType", patient.getBloodType());
        profile.put("height", patient.getHeight());
        profile.put("weight", patient.getWeight());
        profile.put("allergies", patient.getAllergies());
        profile.put("status", patient.getStatus());
        
        // Get user info
        User user = userRepository.findById(patient.getUserId()).orElse(null);
        if (user != null) {
            profile.put("userId", user.getUserId());
            profile.put("username", user.getUsername());
            profile.put("fullName", user.getFullName());
            profile.put("email", user.getEmail());
            profile.put("phone", user.getPhone());
            profile.put("gender", user.getGender());
            profile.put("address", user.getAddress());
            profile.put("dateOfBirth", user.getDateOfBirth());
        }
        
        // Get insurance info
        if (patient.getInsuranceId() != null) {
            profile.put("insuranceId", patient.getInsuranceId());
        }

        return ResponseEntity.ok(profile);
    }

    // PUT /api/patient/profile - Update own profile
    @PutMapping("/profile")
    @PreAuthorize("hasRole('PATIENT')")
    public ResponseEntity<?> updateProfile(Authentication auth, @RequestBody Map<String, Object> updates) {
        Integer patientId = getPatientIdFromAuth(auth);
        if (patientId == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy hồ sơ bệnh nhân"));
        }

        Patient patient = patientRepository.findById(patientId).orElse(null);
        if (patient == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy bệnh nhân"));
        }

        // Update patient fields
        if (updates.containsKey("bloodType")) patient.setBloodType((String) updates.get("bloodType"));
        if (updates.containsKey("height")) patient.setHeight(new java.math.BigDecimal(updates.get("height").toString()));
        if (updates.containsKey("weight")) patient.setWeight(new java.math.BigDecimal(updates.get("weight").toString()));
        if (updates.containsKey("allergies")) patient.setAllergies((String) updates.get("allergies"));
        
        patientRepository.save(patient);

        // Update user fields
        User user = userRepository.findById(patient.getUserId()).orElse(null);
        if (user != null) {
            if (updates.containsKey("fullName")) user.setFullName((String) updates.get("fullName"));
            if (updates.containsKey("phone")) user.setPhone((String) updates.get("phone"));
            if (updates.containsKey("address")) user.setAddress((String) updates.get("address"));
            userRepository.save(user);
        }

        return ResponseEntity.ok(new ApiResponse(true, "Cập nhật thông tin thành công"));
    }

    // GET /api/patient/appointments - Get own appointments
    @GetMapping("/appointments")
    @PreAuthorize("hasRole('PATIENT')")
    public ResponseEntity<?> getMyAppointments(Authentication auth) {
        Integer patientId = getPatientIdFromAuth(auth);
        if (patientId == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy hồ sơ bệnh nhân"));
        }

        List<Appointment> appointments = appointmentRepository.findByPatientId(patientId);
        return ResponseEntity.ok(appointments);
    }

    // POST /api/patient/appointments - Book new appointment
    @PostMapping("/appointments")
    @PreAuthorize("hasRole('PATIENT')")
    public ResponseEntity<?> bookAppointment(Authentication auth, @RequestBody Map<String, Object> appointmentData) {
        Integer patientId = getPatientIdFromAuth(auth);
        if (patientId == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy hồ sơ bệnh nhân"));
        }

        try {
            Appointment appointment = Appointment.builder()
                    .patientId(patientId)
                    .doctorId(Integer.parseInt(appointmentData.get("doctorId").toString()))
                    .departmentId(Integer.parseInt(appointmentData.get("departmentId").toString()))
                    .appointmentDate((String) appointmentData.get("appointmentDate"))
                    .timeSlot((String) appointmentData.get("timeSlot"))
                    .reason((String) appointmentData.get("reason"))
                    .appointmentType((String) appointmentData.getOrDefault("appointmentType", "Khám bệnh"))
                    .status("Chờ xác nhận")
                    .isEmergency(Boolean.parseBoolean(appointmentData.getOrDefault("isEmergency", "false").toString()))
                    .createdAt(LocalDateTime.now().format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss")))
                    .build();

            appointment = appointmentRepository.save(appointment);
            
            Map<String, Object> response = new HashMap<>();
            response.put("success", true);
            response.put("message", "Đặt lịch khám thành công");
            response.put("appointmentId", appointment.getAppointmentId());
            
            return ResponseEntity.ok(response);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Lỗi: " + e.getMessage()));
        }
    }

    // DELETE /api/patient/appointments/{id} - Cancel own appointment
    @DeleteMapping("/appointments/{id}")
    @PreAuthorize("hasRole('PATIENT')")
    public ResponseEntity<?> cancelAppointment(Authentication auth, @PathVariable Integer id) {
        Integer patientId = getPatientIdFromAuth(auth);
        if (patientId == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy hồ sơ bệnh nhân"));
        }

        Appointment appointment = appointmentRepository.findById(id).orElse(null);
        if (appointment == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy lịch hẹn"));
        }

        // Only allow canceling own appointments
        if (!appointment.getPatientId().equals(patientId)) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Bạn không có quyền hủy lịch hẹn này"));
        }

        // Only allow canceling pending appointments
        if (!"Chờ xác nhận".equals(appointment.getStatus())) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không thể hủy lịch hẹn đã được xác nhận"));
        }

        appointment.setStatus("Đã hủy");
        appointmentRepository.save(appointment);

        return ResponseEntity.ok(new ApiResponse(true, "Hủy lịch hẹn thành công"));
    }

    // GET /api/patient/medical-records - Get own medical records
    @GetMapping("/medical-records")
    @PreAuthorize("hasRole('PATIENT')")
    public ResponseEntity<?> getMyMedicalRecords(Authentication auth) {
        Integer patientId = getPatientIdFromAuth(auth);
        if (patientId == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy hồ sơ bệnh nhân"));
        }

        List<MedicalRecord> records = medicalRecordRepository.findByPatientId(patientId);
        return ResponseEntity.ok(records);
    }

    // GET /api/patient/prescriptions - Get own prescriptions
    @GetMapping("/prescriptions")
    @PreAuthorize("hasRole('PATIENT')")
    public ResponseEntity<?> getMyPrescriptions(Authentication auth) {
        Integer patientId = getPatientIdFromAuth(auth);
        if (patientId == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy hồ sơ bệnh nhân"));
        }

        // Get prescriptions through medical records
        List<MedicalRecord> records = medicalRecordRepository.findByPatientId(patientId);
        List<Integer> recordIds = records.stream().map(MedicalRecord::getRecordId).toList();
        List<Prescription> prescriptions = prescriptionRepository.findByRecordIdIn(recordIds);
        
        return ResponseEntity.ok(prescriptions);
    }

    // GET /api/patient/payments - Get own payments
    @GetMapping("/payments")
    @PreAuthorize("hasRole('PATIENT')")
    public ResponseEntity<?> getMyPayments(Authentication auth) {
        Integer patientId = getPatientIdFromAuth(auth);
        if (patientId == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy hồ sơ bệnh nhân"));
        }

        List<Payment> payments = paymentRepository.findByPatientId(patientId);
        return ResponseEntity.ok(payments);
    }

    // GET /api/patient/doctors - Get available doctors
    @GetMapping("/doctors")
    @PreAuthorize("hasRole('PATIENT')")
    public ResponseEntity<?> getAvailableDoctors() {
        List<Doctor> doctors = doctorRepository.findByStatus("Hoạt động");
        return ResponseEntity.ok(doctors);
    }

    // GET /api/patient/departments - Get departments
    @GetMapping("/departments")
    @PreAuthorize("hasRole('PATIENT')")
    public ResponseEntity<?> getDepartments() {
        List<Department> departments = departmentRepository.findByStatus("Hoạt động");
        return ResponseEntity.ok(departments);
    }

    // GET /api/patient/time-slots - Get available time slots for a doctor on a date
    @GetMapping("/time-slots")
    @PreAuthorize("hasRole('PATIENT')")
    public ResponseEntity<?> getTimeSlots(@RequestParam Integer doctorId, @RequestParam String date) {
        List<String> allSlots = List.of("08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00",
                                        "14:00", "14:30", "15:00", "15:30", "16:00");
        
        // Get booked appointments
        List<Appointment> booked = appointmentRepository.findByDoctorIdAndAppointmentDate(doctorId, date);
        List<String> bookedSlots = booked.stream()
                .map(Appointment::getTimeSlot)
                .filter(ts -> ts != null)
                .toList();
        
        // Filter out booked slots
        List<String> availableSlots = allSlots.stream()
                .filter(slot -> !bookedSlots.contains(slot))
                .toList();
        
        return ResponseEntity.ok(availableSlots);
    }
}
