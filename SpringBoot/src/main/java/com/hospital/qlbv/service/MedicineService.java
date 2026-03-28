package com.hospital.qlbv.service;

import com.hospital.qlbv.entity.Medicine;
import com.hospital.qlbv.repository.MedicineRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;

@Service
public class MedicineService {

    private final MedicineRepository medicineRepository;

    public MedicineService(MedicineRepository medicineRepository) {
        this.medicineRepository = medicineRepository;
    }

    public List<Medicine> getAllMedicines() {
        return medicineRepository.findAll();
    }

    public Medicine getMedicineById(Integer id) {
        return medicineRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Medicine not found"));
    }

    public List<Medicine> searchMedicines(String keyword) {
        return medicineRepository.findAll().stream()
                .filter(m -> (m.getMedicineName() != null && m.getMedicineName().contains(keyword)) ||
                            (m.getGenericName() != null && m.getGenericName().contains(keyword)))
                .toList();
    }

    @Transactional
    public Medicine createMedicine(Medicine medicine) {
        medicine.setStatus("Hoạt động");
        return medicineRepository.save(medicine);
    }

    @Transactional
    public Medicine updateMedicine(Integer id, Medicine medicineData) {
        Medicine medicine = medicineRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Medicine not found"));
        
        if (medicineData.getMedicineName() != null) medicine.setMedicineName(medicineData.getMedicineName());
        if (medicineData.getGenericName() != null) medicine.setGenericName(medicineData.getGenericName());
        if (medicineData.getManufacturer() != null) medicine.setManufacturer(medicineData.getManufacturer());
        if (medicineData.getUnit() != null) medicine.setUnit(medicineData.getUnit());
        if (medicineData.getPrice() != null) medicine.setPrice(medicineData.getPrice());
        if (medicineData.getStock() != null) medicine.setStock(medicineData.getStock());
        if (medicineData.getDosageForm() != null) medicine.setDosageForm(medicineData.getDosageForm());
        if (medicineData.getStatus() != null) medicine.setStatus(medicineData.getStatus());
        
        return medicineRepository.save(medicine);
    }

    @Transactional
    public void deleteMedicine(Integer id) {
        medicineRepository.deleteById(id);
    }
}
