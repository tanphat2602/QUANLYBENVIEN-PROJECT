package com.hospital.qlbv.controller;

import com.hospital.qlbv.dto.ApiResponse;
import com.hospital.qlbv.entity.*;
import com.hospital.qlbv.repository.*;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.*;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/pharmacist")
public class PharmacistPortalController {

    private final UserRepository userRepository;
    private final MedicineRepository medicineRepository;
    private final PrescriptionRepository prescriptionRepository;
    private final PrescriptionDetailRepository prescriptionDetailRepository;
    private final PatientRepository patientRepository;
    private final MedicalRecordRepository medicalRecordRepository;

    public PharmacistPortalController(UserRepository userRepository, MedicineRepository medicineRepository,
                                   PrescriptionRepository prescriptionRepository,
                                   PrescriptionDetailRepository prescriptionDetailRepository,
                                   PatientRepository patientRepository,
                                   MedicalRecordRepository medicalRecordRepository) {
        this.userRepository = userRepository;
        this.medicineRepository = medicineRepository;
        this.prescriptionRepository = prescriptionRepository;
        this.prescriptionDetailRepository = prescriptionDetailRepository;
        this.patientRepository = patientRepository;
        this.medicalRecordRepository = medicalRecordRepository;
    }

    // GET /api/pharmacist/medicines - Get all medicines
    @GetMapping("/medicines")
    @PreAuthorize("hasRole('PHARMACIST')")
    public ResponseEntity<?> getMedicines() {
        List<Medicine> medicines = medicineRepository.findAll();
        return ResponseEntity.ok(medicines);
    }

    // GET /api/pharmacist/medicines/low-stock - Get low stock medicines
    @GetMapping("/medicines/low-stock")
    @PreAuthorize("hasRole('PHARMACIST')")
    public ResponseEntity<?> getLowStockMedicines() {
        List<Medicine> medicines = medicineRepository.findByStatus("Hoạt động");
        List<Medicine> lowStock = medicines.stream()
                .filter(m -> m.getStock() < 100)
                .toList();
        return ResponseEntity.ok(lowStock);
    }

    // POST /api/pharmacist/medicines - Add new medicine
    @PostMapping("/medicines")
    @PreAuthorize("hasRole('PHARMACIST')")
    public ResponseEntity<?> addMedicine(@RequestBody Map<String, Object> medicineData) {
        try {
            Medicine medicine = Medicine.builder()
                    .medicineName((String) medicineData.get("medicineName"))
                    .genericName((String) medicineData.getOrDefault("genericName", ""))
                    .manufacturer((String) medicineData.getOrDefault("manufacturer", ""))
                    .unit((String) medicineData.getOrDefault("unit", "Viên"))
                    .price(new BigDecimal(medicineData.get("price").toString()))
                    .stock(Integer.parseInt(medicineData.getOrDefault("stock", "0").toString()))
                    .dosageForm((String) medicineData.getOrDefault("dosageForm", "Viên nén"))
                    .status("Hoạt động")
                    .build();

            medicine = medicineRepository.save(medicine);

            Map<String, Object> response = new HashMap<>();
            response.put("success", true);
            response.put("message", "Thêm thuốc thành công");
            response.put("medicineId", medicine.getMedicineId());
            
            return ResponseEntity.ok(response);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Lỗi: " + e.getMessage()));
        }
    }

    // PUT /api/pharmacist/medicines/{id} - Update medicine
    @PutMapping("/medicines/{id}")
    @PreAuthorize("hasRole('PHARMACIST')")
    public ResponseEntity<?> updateMedicine(@PathVariable Integer id, @RequestBody Map<String, Object> medicineData) {
        Medicine medicine = medicineRepository.findById(id).orElse(null);
        if (medicine == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy thuốc"));
        }

        if (medicineData.containsKey("medicineName")) medicine.setMedicineName((String) medicineData.get("medicineName"));
        if (medicineData.containsKey("genericName")) medicine.setGenericName((String) medicineData.get("genericName"));
        if (medicineData.containsKey("manufacturer")) medicine.setManufacturer((String) medicineData.get("manufacturer"));
        if (medicineData.containsKey("unit")) medicine.setUnit((String) medicineData.get("unit"));
        if (medicineData.containsKey("price")) medicine.setPrice(new BigDecimal(medicineData.get("price").toString()));
        if (medicineData.containsKey("stock")) medicine.setStock(Integer.parseInt(medicineData.get("stock").toString()));
        if (medicineData.containsKey("dosageForm")) medicine.setDosageForm((String) medicineData.get("dosageForm"));
        if (medicineData.containsKey("status")) medicine.setStatus((String) medicineData.get("status"));

        medicineRepository.save(medicine);

        return ResponseEntity.ok(new ApiResponse(true, "Cập nhật thuốc thành công"));
    }

    // DELETE /api/pharmacist/medicines/{id} - Delete medicine
    @DeleteMapping("/medicines/{id}")
    @PreAuthorize("hasRole('PHARMACIST')")
    public ResponseEntity<?> deleteMedicine(@PathVariable Integer id) {
        Medicine medicine = medicineRepository.findById(id).orElse(null);
        if (medicine == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy thuốc"));
        }

        medicine.setStatus("Ngừng hoạt động");
        medicineRepository.save(medicine);

        return ResponseEntity.ok(new ApiResponse(true, "Xóa thuốc thành công"));
    }

    // GET /api/pharmacist/prescriptions/pending - Get pending prescriptions
    @GetMapping("/prescriptions/pending")
    @PreAuthorize("hasRole('PHARMACIST')")
    public ResponseEntity<?> getPendingPrescriptions() {
        List<Prescription> prescriptions = prescriptionRepository.findAll();
        List<Prescription> pending = prescriptions.stream()
                .filter(p -> "Chờ cấp phát".equals(p.getStatus()))
                .toList();
        
        // Add patient and record info
        List<Map<String, Object>> result = pending.stream().map(p -> {
            Map<String, Object> item = new HashMap<>();
            item.put("prescriptionId", p.getPrescriptionId());
            item.put("diagnosis", p.getDiagnosis());
            item.put("status", p.getStatus());
            item.put("createdAt", p.getCreatedAt());
            
            // Get patient info through medical record
            medicalRecordRepository.findById(p.getRecordId()).ifPresent(mr -> {
                item.put("recordId", mr.getRecordId());
                patientRepository.findById(mr.getPatientId()).ifPresent(patient -> {
                    item.put("patientId", patient.getPatientId());
                    userRepository.findById(patient.getUserId()).ifPresent(user -> {
                        item.put("patientName", user.getFullName());
                        item.put("patientPhone", user.getPhone());
                    });
                });
            });
            
            // Get prescription details (medicines)
            List<PrescriptionDetail> details = prescriptionDetailRepository.findByPrescriptionId(p.getPrescriptionId());
            List<Map<String, Object>> meds = details.stream().map(d -> {
                Map<String, Object> med = new HashMap<>();
                med.put("detailId", d.getDetailId());
                med.put("dosage", d.getDosage());
                med.put("quantity", d.getQuantity());
                    med.put("instructions", d.getUsageInstructions());
                
                medicineRepository.findById(d.getMedicineId()).ifPresent(m -> {
                    med.put("medicineId", m.getMedicineId());
                    med.put("medicineName", m.getMedicineName());
                    med.put("stock", m.getStock());
                });
                
                return med;
            }).toList();
            item.put("medicines", meds);
            
            return item;
        }).toList();

        return ResponseEntity.ok(result);
    }

    // PUT /api/pharmacist/prescriptions/{id}/dispense - Dispense prescription
    @PutMapping("/prescriptions/{id}/dispense")
    @PreAuthorize("hasRole('PHARMACIST')")
    public ResponseEntity<?> dispensePrescription(@PathVariable Integer id) {
        Prescription prescription = prescriptionRepository.findById(id).orElse(null);
        if (prescription == null) {
            return ResponseEntity.badRequest().body(new ApiResponse(false, "Không tìm thấy đơn thuốc"));
        }

        // Check stock and update
        List<PrescriptionDetail> details = prescriptionDetailRepository.findByPrescriptionId(id);
        for (PrescriptionDetail detail : details) {
            Medicine medicine = medicineRepository.findById(detail.getMedicineId()).orElse(null);
            if (medicine != null) {
                int qty = Integer.parseInt(detail.getQuantity());
                int newStock = medicine.getStock() - qty;
                if (newStock < 0) {
                    return ResponseEntity.badRequest().body(new ApiResponse(false, 
                            "Không đủ thuốc: " + medicine.getMedicineName()));
                }
                medicine.setStock(newStock);
                medicineRepository.save(medicine);
            }
        }

        prescription.setStatus("Đã cấp phát");
        prescriptionRepository.save(prescription);

        return ResponseEntity.ok(new ApiResponse(true, "Cấp phát thuốc thành công"));
    }

    // GET /api/pharmacist/prescriptions/history - Get prescription history
    @GetMapping("/prescriptions/history")
    @PreAuthorize("hasRole('PHARMACIST')")
    public ResponseEntity<?> getPrescriptionHistory() {
        List<Prescription> prescriptions = prescriptionRepository.findAll();
        List<Prescription> dispensed = prescriptions.stream()
                .filter(p -> "Đã cấp phát".equals(p.getStatus()))
                .toList();
        
        return ResponseEntity.ok(dispensed);
    }

    // GET /api/pharmacist/reports/inventory - Get inventory report
    @GetMapping("/reports/inventory")
    @PreAuthorize("hasRole('PHARMACIST')")
    public ResponseEntity<?> getInventoryReport() {
        List<Medicine> medicines = medicineRepository.findByStatus("Hoạt động");
        
        Map<String, Object> report = new HashMap<>();
        report.put("totalMedicines", medicines.size());
        report.put("totalValue", medicines.stream()
                .map(m -> m.getPrice().multiply(BigDecimal.valueOf(m.getStock())))
                .reduce(BigDecimal.ZERO, BigDecimal::add));
        report.put("lowStockCount", medicines.stream().filter(m -> m.getStock() < 100).count());
        report.put("outOfStockCount", medicines.stream().filter(m -> m.getStock() == 0).count());
        report.put("medicines", medicines);

        return ResponseEntity.ok(report);
    }
}
