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
@RequestMapping("/api/doctor")
public class DoctorPortalController {

    private final UserRepository userRepository;
    private final DoctorRepository doctorRepository;
    private final PatientRepository patientRepository;
    private final AppointmentRepository appointmentRepository;
    private final MedicalRecordRepository medicalRecordRepository;
    private final PrescriptionRepository prescriptionRepository;
    private final PrescriptionDetailRepository prescriptionDetailRepository;
    private final MedicineRepository medicineRepository;
    private final DoctorScheduleRepository doctorScheduleRepository;

    public DoctorPortalController(UserRepository userRepository, DoctorRepository doctorRepository,
                                 PatientRepository patientRepository, AppointmentRepository appointmentRepository,
                                 MedicalRecordRepository medicalRecordRepository,
                                 PrescriptionRepository prescriptionRepository,
                                 PrescriptionDetailRepository prescriptionDetailRepository,
                                 MedicineRepository medicineRepository,
                                 DoctorScheduleRepository doctorScheduleRepository) {
        this.userRepository = userRepository;
        this.doctorRepository = doctorRepository;
        this.patientRepository = patientRepository;
        this.appointmentRepository = appointmentRepository;
        this.medicalRecordRepository = medicalRecordRepository;
        this.prescriptionRepository = prescriptionRepository;
        this.prescriptionDetailRepository = prescriptionDetailRepository;
        this.medicineRepository = medicineRepository;
        this.doctorScheduleRepository = doctorScheduleRepository;
    }

    // Helper to get doctor ID from authentication
    private Integer getDoctorIdFromAuth(Authentication auth) {
        String username = auth.getName();
        User user = userRepository.findByUsername(username).orElse(null);
        if (user == null) return null;
        
        return doctorRepository.findByUserId(user.getUserId())
                .map(Doctor::getDoctorId)
                .orElse(null);
    }

    // GET /api/doctor/profile - Get own profile
    @GetMapping("/profile")
    @PreAuthorize("hasRole('DOCTOR')")
    public ResponseEntity<?> getProfile(Authentication auth) {
        Integer doctorId = getDoctorIdFromAuth(auth);
        if (doctorId == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy hồ sơ bác sĩ"));
        }

        Doctor doctor = doctorRepository.findById(doctorId).orElse(null);
        if (doctor == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy bác sĩ"));
        }

        Map<String, Object> profile = new HashMap<>();
        profile.put("doctorId", doctor.getDoctorId());
        profile.put("specialty", doctor.getSpecialty());
        profile.put("experience", doctor.getExperience());
        profile.put("education", doctor.getEducation());
        profile.put("consultationFee", doctor.getConsultationFee());
        profile.put("workingSchedule", doctor.getWorkingSchedule());
        profile.put("status", doctor.getStatus());
        
        // Get user info
        User user = userRepository.findById(doctor.getUserId()).orElse(null);
        if (user != null) {
            profile.put("userId", user.getUserId());
            profile.put("username", user.getUsername());
            profile.put("fullName", user.getFullName());
            profile.put("email", user.getEmail());
            profile.put("phone", user.getPhone());
            profile.put("gender", user.getGender());
        }

        return ResponseEntity.ok(profile);
    }

    // GET /api/doctor/schedule - Get today's appointments
    @GetMapping("/schedule")
    @PreAuthorize("hasRole('DOCTOR')")
    public ResponseEntity<?> getTodaySchedule(Authentication auth) {
        Integer doctorId = getDoctorIdFromAuth(auth);
        if (doctorId == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy hồ sơ bác sĩ"));
        }

        String today = LocalDate.now().toString();
        List<Appointment> appointments = appointmentRepository.findByDoctorIdAndAppointmentDate(doctorId, today);
        
        return ResponseEntity.ok(appointments);
    }

    // GET /api/doctor/appointments - Get all appointments
    @GetMapping("/appointments")
    @PreAuthorize("hasRole('DOCTOR')")
    public ResponseEntity<?> getAllAppointments(Authentication auth,
            @RequestParam(required = false) String date,
            @RequestParam(required = false) String status) {
        Integer doctorId = getDoctorIdFromAuth(auth);
        if (doctorId == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy hồ sơ bác sĩ"));
        }

        List<Appointment> appointments;
        if (date != null) {
            appointments = appointmentRepository.findByDoctorIdAndAppointmentDate(doctorId, date);
        } else if (status != null) {
            appointments = appointmentRepository.findByDoctorId(doctorId);
            appointments = appointments.stream()
                    .filter(a -> status.equals(a.getStatus()))
                    .toList();
        } else {
            appointments = appointmentRepository.findByDoctorId(doctorId);
        }
        
        return ResponseEntity.ok(appointments);
    }

    // GET /api/doctor/patients - Get assigned patients
    @GetMapping("/patients")
    @PreAuthorize("hasRole('DOCTOR')")
    public ResponseEntity<?> getAssignedPatients(Authentication auth) {
        Integer doctorId = getDoctorIdFromAuth(auth);
        if (doctorId == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy hồ sơ bác sĩ"));
        }

        // Get patients from appointments
        List<Appointment> appointments = appointmentRepository.findByDoctorId(doctorId);
        List<Integer> patientIds = appointments.stream()
                .map(Appointment::getPatientId)
                .distinct()
                .toList();

        List<Map<String, Object>> patients = patientIds.stream()
                .map(id -> {
                    Patient patient = patientRepository.findById(id).orElse(null);
                    if (patient == null) return null;
                    
                    Map<String, Object> p = new HashMap<>();
                    p.put("patientId", patient.getPatientId());
                    p.put("bloodType", patient.getBloodType());
                    p.put("height", patient.getHeight());
                    p.put("weight", patient.getWeight());
                    p.put("allergies", patient.getAllergies());
                    p.put("status", patient.getStatus());
                    
                    User user = userRepository.findById(patient.getUserId()).orElse(null);
                    if (user != null) {
                        p.put("fullName", user.getFullName());
                        p.put("phone", user.getPhone());
                        p.put("gender", user.getGender());
                        p.put("dateOfBirth", user.getDateOfBirth());
                    }
                    return p;
                })
                .filter(p -> p != null)
                .toList();

        return ResponseEntity.ok(patients);
    }

    // GET /api/doctor/patient/{id} - Get patient details
    @GetMapping("/patient/{id}")
    @PreAuthorize("hasRole('DOCTOR')")
    public ResponseEntity<?> getPatientDetails(@PathVariable Integer id) {
        Patient patient = patientRepository.findById(id).orElse(null);
        if (patient == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy bệnh nhân"));
        }

        Map<String, Object> details = new HashMap<>();
        details.put("patientId", patient.getPatientId());
        details.put("bloodType", patient.getBloodType());
        details.put("height", patient.getHeight());
        details.put("weight", patient.getWeight());
        details.put("allergies", patient.getAllergies());
        
        User user = userRepository.findById(patient.getUserId()).orElse(null);
        if (user != null) {
            details.put("fullName", user.getFullName());
            details.put("phone", user.getPhone());
            details.put("gender", user.getGender());
            details.put("dateOfBirth", user.getDateOfBirth());
            details.put("address", user.getAddress());
        }

        // Get medical history
        List<MedicalRecord> records = medicalRecordRepository.findByPatientId(id);
        details.put("medicalRecords", records);

        return ResponseEntity.ok(details);
    }

    // POST /api/doctor/medical-record - Create medical record
    @PostMapping("/medical-record")
    @PreAuthorize("hasRole('DOCTOR')")
    public ResponseEntity<?> createMedicalRecord(Authentication auth, @RequestBody Map<String, Object> recordData) {
        Integer doctorId = getDoctorIdFromAuth(auth);
        if (doctorId == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy hồ sơ bác sĩ"));
        }

        try {
            MedicalRecord record = MedicalRecord.builder()
                    .patientId(Integer.parseInt(recordData.get("patientId").toString()))
                    .doctorId(doctorId)
                    .appointmentId(recordData.containsKey("appointmentId") ? 
                            Integer.parseInt(recordData.get("appointmentId").toString()) : null)
                    .symptoms((String) recordData.get("symptoms"))
                    .diagnosis((String) recordData.get("diagnosis"))
                    .treatment((String) recordData.get("treatment"))
                    .notes((String) recordData.getOrDefault("notes", ""))
                    .vitalSigns((String) recordData.getOrDefault("vitalSigns", ""))
                    .status("Hoàn thành")
                    .dateCreated(LocalDateTime.now().format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss")))
                    .build();

            record = medicalRecordRepository.save(record);

            // Update appointment status
            if (record.getAppointmentId() != null) {
                appointmentRepository.findById(record.getAppointmentId()).ifPresent(a -> {
                    a.setStatus("Hoàn thành");
                    appointmentRepository.save(a);
                });
            }

            Map<String, Object> response = new HashMap<>();
            response.put("success", true);
            response.put("message", "Tạo bệnh án thành công");
            response.put("recordId", record.getRecordId());
            
            return ResponseEntity.ok(response);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Lỗi: " + e.getMessage()));
        }
    }

    // POST /api/doctor/prescription - Create prescription
    @PostMapping("/prescription")
    @PreAuthorize("hasRole('DOCTOR')")
    public ResponseEntity<?> createPrescription(Authentication auth, @RequestBody Map<String, Object> prescriptionData) {
        Integer doctorId = getDoctorIdFromAuth(auth);
        if (doctorId == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy hồ sơ bác sĩ"));
        }

        try {
            // Create prescription
            Prescription prescription = Prescription.builder()
                    .recordId(Integer.parseInt(prescriptionData.get("recordId").toString()))
                    .diagnosis((String) prescriptionData.get("diagnosis"))
                    .status("Chờ cấp phát")
                    .createdAt(LocalDateTime.now().format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss")))
                    .build();

            prescription = prescriptionRepository.save(prescription);

            // Add prescription details (medicines)
            if (prescriptionData.containsKey("medicines")) {
                List<Map<String, Object>> medicines = (List<Map<String, Object>>) prescriptionData.get("medicines");
                for (Map<String, Object> med : medicines) {
                    PrescriptionDetail detail = PrescriptionDetail.builder()
                            .prescriptionId(prescription.getPrescriptionId())
                            .medicineId(Integer.parseInt(med.get("medicineId").toString()))
                            .dosage((String) med.get("dosage"))
                            .quantity(Integer.parseInt(med.get("quantity").toString()))
                            .instructions((String) med.get("instructions"))
                            .build();
                    prescriptionDetailRepository.save(detail);
                }
            }

            Map<String, Object> response = new HashMap<>();
            response.put("success", true);
            response.put("message", "Kê đơn thuốc thành công");
            response.put("prescriptionId", prescription.getPrescriptionId());
            
            return ResponseEntity.ok(response);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Lỗi: " + e.getMessage()));
        }
    }

    // GET /api/doctor/medicines - Get available medicines
    @GetMapping("/medicines")
    @PreAuthorize("hasRole('DOCTOR')")
    public ResponseEntity<?> getMedicines() {
        List<Medicine> medicines = medicineRepository.findByStatus("Hoạt động");
        return ResponseEntity.ok(medicines);
    }

    // PUT /api/doctor/appointment/{id}/status - Update appointment status
    @PutMapping("/appointment/{id}/status")
    @PreAuthorize("hasRole('DOCTOR')")
    public ResponseEntity<?> updateAppointmentStatus(@PathVariable Integer id, @RequestBody Map<String, String> data) {
        Appointment appointment = appointmentRepository.findById(id).orElse(null);
        if (appointment == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy lịch hẹn"));
        }

        appointment.setStatus(data.get("status"));
        appointmentRepository.save(appointment);

        return ResponseEntity.ok(new ApiResponse(true, "Cập nhật trạng thái thành công"));
    }
}
