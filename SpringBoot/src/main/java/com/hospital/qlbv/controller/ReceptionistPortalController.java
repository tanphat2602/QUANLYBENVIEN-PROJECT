package com.hospital.qlbv.controller;

import com.hospital.qlbv.dto.ApiResponse;
import com.hospital.qlbv.entity.*;
import com.hospital.qlbv.repository.*;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.*;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/receptionist")
public class ReceptionistPortalController {

    private final UserRepository userRepository;
    private final PatientRepository patientRepository;
    private final DoctorRepository doctorRepository;
    private final AppointmentRepository appointmentRepository;
    private final EmergencyRequestRepository emergencyRequestRepository;
    private final PaymentRepository paymentRepository;
    private final DepartmentRepository departmentRepository;

    public ReceptionistPortalController(UserRepository userRepository, PatientRepository patientRepository,
                                     DoctorRepository doctorRepository, AppointmentRepository appointmentRepository,
                                     EmergencyRequestRepository emergencyRequestRepository,
                                     PaymentRepository paymentRepository, DepartmentRepository departmentRepository) {
        this.userRepository = userRepository;
        this.patientRepository = patientRepository;
        this.doctorRepository = doctorRepository;
        this.appointmentRepository = appointmentRepository;
        this.emergencyRequestRepository = emergencyRequestRepository;
        this.paymentRepository = paymentRepository;
        this.departmentRepository = departmentRepository;
    }

    // GET /api/receptionist/queue - Get today's queue
    @GetMapping("/queue")
    @PreAuthorize("hasRole('RECEPTIONIST')")
    public ResponseEntity<?> getTodayQueue() {
        String today = LocalDate.now().toString();
        List<Appointment> appointments = appointmentRepository.findByAppointmentDate(today);
        
        // Sort by status priority
        appointments.sort((a, b) -> {
            if ("Đang khám".equals(a.getStatus())) return -1;
            if ("Đang khám".equals(b.getStatus())) return 1;
            if ("Đã xác nhận".equals(a.getStatus()) && !"Đã xác nhận".equals(b.getStatus())) return -1;
            if ("Đã xác nhận".equals(b.getStatus()) && !"Đã xác nhận".equals(a.getStatus())) return 1;
            return 0;
        });
        
        // Add patient and doctor info
        List<Map<String, Object>> queue = appointments.stream().map(a -> {
            Map<String, Object> item = new HashMap<>();
            item.put("appointmentId", a.getAppointmentId());
            item.put("appointmentDate", a.getAppointmentDate());
            item.put("timeSlot", a.getTimeSlot());
            item.put("status", a.getStatus());
            item.put("reason", a.getReason());
            item.put("isEmergency", a.getIsEmergency());
            
            // Patient info
            patientRepository.findById(a.getPatientId()).ifPresent(p -> {
                item.put("patientId", p.getPatientId());
                userRepository.findById(p.getUserId()).ifPresent(u -> {
                    item.put("patientName", u.getFullName());
                    item.put("patientPhone", u.getPhone());
                });
            });
            
            // Doctor info
            doctorRepository.findById(a.getDoctorId()).ifPresent(d -> {
                item.put("doctorId", d.getDoctorId());
                userRepository.findById(d.getUserId()).ifPresent(u -> {
                    item.put("doctorName", u.getFullName());
                });
                item.put("specialty", d.getSpecialty());
            });
            
            return item;
        }).toList();

        return ResponseEntity.ok(queue);
    }

    // POST /api/receptionist/checkin - Check in patient
    @PostMapping("/checkin")
    @PreAuthorize("hasRole('RECEPTIONIST')")
    public ResponseEntity<?> checkIn(@RequestBody Map<String, Integer> data) {
        Integer appointmentId = data.get("appointmentId");
        
        Appointment appointment = appointmentRepository.findById(appointmentId).orElse(null);
        if (appointment == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy lịch hẹn"));
        }

        appointment.setStatus("Đã xác nhận");
        appointmentRepository.save(appointment);

        return ResponseEntity.ok(new ApiResponse(true, "Check-in thành công"));
    }

    // PUT /api/receptionist/appointment/{id}/call - Call next patient
    @PutMapping("/appointment/{id}/call")
    @PreAuthorize("hasRole('RECEPTIONIST')")
    public ResponseEntity<?> callPatient(@PathVariable Integer id) {
        Appointment appointment = appointmentRepository.findById(id).orElse(null);
        if (appointment == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy lịch hẹn"));
        }

        appointment.setStatus("Đang khám");
        appointmentRepository.save(appointment);

        return ResponseEntity.ok(new ApiResponse(true, "Đã gọi bệnh nhân"));
    }

    // GET /api/receptionist/appointments - Get all appointments
    @GetMapping("/appointments")
    @PreAuthorize("hasRole('RECEPTIONIST')")
    public ResponseEntity<?> getAppointments(@RequestParam(required = false) String date,
                                           @RequestParam(required = false) String status) {
        List<Appointment> appointments;
        
        if (date != null) {
            appointments = appointmentRepository.findByAppointmentDate(date);
        } else if (status != null) {
            appointments = appointmentRepository.findByStatus(status);
        } else {
            appointments = appointmentRepository.findAll();
        }
        
        return ResponseEntity.ok(appointments);
    }

    // POST /api/receptionist/appointments - Create appointment
    @PostMapping("/appointments")
    @PreAuthorize("hasRole('RECEPTIONIST')")
    public ResponseEntity<?> createAppointment(@RequestBody Map<String, Object> appointmentData) {
        try {
            Appointment appointment = Appointment.builder()
                    .patientId(Integer.parseInt(appointmentData.get("patientId").toString()))
                    .doctorId(Integer.parseInt(appointmentData.get("doctorId").toString()))
                    .departmentId(Integer.parseInt(appointmentData.get("departmentId").toString()))
                    .appointmentDate((String) appointmentData.get("appointmentDate"))
                    .timeSlot((String) appointmentData.get("timeSlot"))
                    .reason((String) appointmentData.get("reason"))
                    .appointmentType((String) appointmentData.getOrDefault("appointmentType", "Khám bệnh"))
                    .status("Đã xác nhận")
                    .isEmergency(Boolean.parseBoolean(appointmentData.getOrDefault("isEmergency", "false").toString()))
                    .createdAt(LocalDateTime.now().format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss")))
                    .build();

            appointment = appointmentRepository.save(appointment);
            
            Map<String, Object> response = new HashMap<>();
            response.put("success", true);
            response.put("message", "Tạo lịch hẹn thành công");
            response.put("appointmentId", appointment.getAppointmentId());
            
            return ResponseEntity.ok(response);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Lỗi: " + e.getMessage()));
        }
    }

    // PUT /api/receptionist/appointment/{id} - Update appointment
    @PutMapping("/appointment/{id}")
    @PreAuthorize("hasRole('RECEPTIONIST')")
    public ResponseEntity<?> updateAppointment(@PathVariable Integer id, @RequestBody Map<String, Object> data) {
        Appointment appointment = appointmentRepository.findById(id).orElse(null);
        if (appointment == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy lịch hẹn"));
        }

        if (data.containsKey("doctorId")) appointment.setDoctorId(Integer.parseInt(data.get("doctorId").toString()));
        if (data.containsKey("appointmentDate")) appointment.setAppointmentDate((String) data.get("appointmentDate"));
        if (data.containsKey("timeSlot")) appointment.setTimeSlot((String) data.get("timeSlot"));
        if (data.containsKey("status")) appointment.setStatus((String) data.get("status"));
        
        appointmentRepository.save(appointment);

        return ResponseEntity.ok(new ApiResponse(true, "Cập nhật thành công"));
    }

    // DELETE /api/receptionist/appointments/{id} - Cancel appointment
    @DeleteMapping("/appointments/{id}")
    @PreAuthorize("hasRole('RECEPTIONIST')")
    public ResponseEntity<?> cancelAppointment(@PathVariable Integer id) {
        Appointment appointment = appointmentRepository.findById(id).orElse(null);
        if (appointment == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy lịch hẹn"));
        }

        appointment.setStatus("Đã hủy");
        appointmentRepository.save(appointment);

        return ResponseEntity.ok(new ApiResponse(true, "Hủy lịch hẹn thành công"));
    }

    // GET /api/receptionist/patients - Get all patients
    @GetMapping("/patients")
    @PreAuthorize("hasRole('RECEPTIONIST')")
    public ResponseEntity<?> getPatients() {
        List<Patient> patients = patientRepository.findAll();
        return ResponseEntity.ok(patients);
    }

    // POST /api/receptionist/patients - Create patient
    @PostMapping("/patients")
    @PreAuthorize("hasRole('RECEPTIONIST')")
    public ResponseEntity<?> createPatient(@RequestBody Map<String, Object> patientData) {
        try {
            // Create user first
            User user = User.builder()
                    .username((String) patientData.get("username"))
                    .passwordHash("$2a$10$dummy") // Temporary, should be set by patient
                    .email((String) patientData.get("email"))
                    .fullName((String) patientData.get("fullName"))
                    .phone((String) patientData.get("phone"))
                    .gender((String) patientData.get("gender"))
                    .address((String) patientData.getOrDefault("address", ""))
                    .dateOfBirth((String) patientData.getOrDefault("dateOfBirth", ""))
                    .status("Hoạt động")
                    .createdAt(LocalDateTime.now().format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss")))
                    .build();

            user = userRepository.save(user);

            // Create patient record
            Patient patient = Patient.builder()
                    .userId(user.getUserId())
                    .bloodType((String) patientData.getOrDefault("bloodType", ""))
                    .height(patientData.containsKey("height") ? 
                            new BigDecimal(patientData.get("height").toString()) : null)
                    .weight(patientData.containsKey("weight") ? 
                            new BigDecimal(patientData.get("weight").toString()) : null)
                    .status("Hoạt động")
                    .createdAt(LocalDateTime.now().format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss")))
                    .build();

            patient = patientRepository.save(patient);

            Map<String, Object> response = new HashMap<>();
            response.put("success", true);
            response.put("message", "Tạo hồ sơ bệnh nhân thành công");
            response.put("patientId", patient.getPatientId());
            response.put("userId", user.getUserId());
            
            return ResponseEntity.ok(response);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Lỗi: " + e.getMessage()));
        }
    }

    // GET /api/receptionist/doctors - Get all doctors
    @GetMapping("/doctors")
    @PreAuthorize("hasRole('RECEPTIONIST')")
    public ResponseEntity<?> getDoctors() {
        List<Doctor> doctors = doctorRepository.findByStatus("Hoạt động");
        return ResponseEntity.ok(doctors);
    }

    // GET /api/receptionist/departments - Get all departments
    @GetMapping("/departments")
    @PreAuthorize("hasRole('RECEPTIONIST')")
    public ResponseEntity<?> getDepartments() {
        List<Department> departments = departmentRepository.findByStatus("Hoạt động");
        return ResponseEntity.ok(departments);
    }

    // POST /api/receptionist/emergency - Create emergency case
    @PostMapping("/emergency")
    @PreAuthorize("hasRole('RECEPTIONIST')")
    public ResponseEntity<?> createEmergency(@RequestBody Map<String, Object> emergencyData) {
        try {
            EmergencyRequest emergency = EmergencyRequest.builder()
                    .patientId(Integer.parseInt(emergencyData.get("patientId").toString()))
                    .reason((String) emergencyData.get("reason"))
                    .priority(Integer.parseInt(emergencyData.getOrDefault("priority", "1").toString()))
                    .status("Chờ tiếp nhận")
                    .emergencyType((String) emergencyData.getOrDefault("emergencyType", "Khẩn cấp"))
                    .createdAt(LocalDateTime.now().format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss")))
                    .build();

            emergency = emergencyRequestRepository.save(emergency);

            return ResponseEntity.ok(new ApiResponse(true, "Tạo ca cấp cứu thành công"));
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Lỗi: " + e.getMessage()));
        }
    }

    // POST /api/receptionist/payment - Create payment
    @PostMapping("/payment")
    @PreAuthorize("hasRole('RECEPTIONIST')")
    public ResponseEntity<?> createPayment(@RequestBody Map<String, Object> paymentData) {
        try {
            Payment payment = Payment.builder()
                    .patientId(Integer.parseInt(paymentData.get("patientId").toString()))
                    .appointmentId(paymentData.containsKey("appointmentId") ? 
                            Integer.parseInt(paymentData.get("appointmentId").toString()) : null)
                    .amount(new BigDecimal(paymentData.get("amount").toString()))
                    .paymentMethod((String) paymentData.getOrDefault("paymentMethod", "Tiền mặt"))
                    .status("Hoàn thành")
                    .paymentDate(LocalDateTime.now().format(DateTimeFormatter.ofPattern("yyyy-MM-dd")))
                    .createdAt(LocalDateTime.now().format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss")))
                    .build();

            payment = paymentRepository.save(payment);

            return ResponseEntity.ok(new ApiResponse(true, "Tạo thanh toán thành công"));
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Lỗi: " + e.getMessage()));
        }
    }
}
