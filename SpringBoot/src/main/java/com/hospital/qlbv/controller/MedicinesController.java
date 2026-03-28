package com.hospital.qlbv.controller;

import com.hospital.qlbv.entity.Medicine;
import com.hospital.qlbv.service.MedicineService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/medicines")
public class MedicinesController {

    private final MedicineService medicineService;

    public MedicinesController(MedicineService medicineService) {
        this.medicineService = medicineService;
    }

    @GetMapping
    public ResponseEntity<List<Medicine>> getAllMedicines() {
        return ResponseEntity.ok(medicineService.getAllMedicines());
    }

    @GetMapping("/{id}")
    public ResponseEntity<Medicine> getMedicineById(@PathVariable Integer id) {
        return ResponseEntity.ok(medicineService.getMedicineById(id));
    }

    @GetMapping("/search/{keyword}")
    public ResponseEntity<List<Medicine>> searchMedicines(@PathVariable String keyword) {
        return ResponseEntity.ok(medicineService.searchMedicines(keyword));
    }

    @PostMapping
    public ResponseEntity<Medicine> createMedicine(@RequestBody Medicine medicine) {
        return ResponseEntity.ok(medicineService.createMedicine(medicine));
    }

    @PutMapping("/{id}")
    public ResponseEntity<Medicine> updateMedicine(@PathVariable Integer id, @RequestBody Medicine medicine) {
        return ResponseEntity.ok(medicineService.updateMedicine(id, medicine));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteMedicine(@PathVariable Integer id) {
        medicineService.deleteMedicine(id);
        return ResponseEntity.ok().body(new MessageResponse("Xóa thành công!"));
    }
}
