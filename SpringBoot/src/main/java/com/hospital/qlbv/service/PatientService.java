package com.hospital.qlbv.service;

import com.hospital.qlbv.dto.PatientDTO;
import com.hospital.qlbv.entity.Patient;
import com.hospital.qlbv.repository.PatientRepository;
import com.hospital.qlbv.repository.UserRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.stream.Collectors;

@Service
public class PatientService {

    private final PatientRepository patientRepository;
    private final UserRepository userRepository;

    public PatientService(PatientRepository patientRepository, UserRepository userRepository) {
        this.patientRepository = patientRepository;
        this.userRepository = userRepository;
    }

    public List<PatientDTO> getAllPatients() {
        return patientRepository.findAll().stream()
                .map(this::toDTO)
                .collect(Collectors.toList());
    }

    public PatientDTO getPatientById(Integer id) {
        Patient patient = patientRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Patient not found"));
        return toDTO(patient);
    }

    @Transactional
    public PatientDTO createPatient(Patient patient) {
        patient = patientRepository.save(patient);
        return toDTO(patient);
    }

    @Transactional
    public PatientDTO updatePatient(Integer id, Patient patientData) {
        Patient patient = patientRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Patient not found"));
        
        if (patientData.getBloodType() != null) patient.setBloodType(patientData.getBloodType());
        if (patientData.getAllergies() != null) patient.setAllergies(patientData.getAllergies());
        if (patientData.getHeight() != null) patient.setHeight(patientData.getHeight());
        if (patientData.getWeight() != null) patient.setWeight(patientData.getWeight());
        if (patientData.getStatus() != null) patient.setStatus(patientData.getStatus());
        
        patient = patientRepository.save(patient);
        return toDTO(patient);
    }

    @Transactional
    public void deletePatient(Integer id) {
        patientRepository.deleteById(id);
    }

    private PatientDTO toDTO(Patient patient) {
        PatientDTO dto = PatientDTO.builder()
                .patientId(patient.getPatientId())
                .userId(patient.getUserId())
                .bloodType(patient.getBloodType())
                .allergies(patient.getAllergies())
                .height(patient.getHeight() != null ? patient.getHeight().doubleValue() : null)
                .weight(patient.getWeight() != null ? patient.getWeight().doubleValue() : null)
                .status(patient.getStatus())
                .createdAt(patient.getCreatedAt())
                .build();

        if (patient.getUser() != null) {
            dto.setFullName(patient.getUser().getFullName());
            dto.setEmail(patient.getUser().getEmail());
            dto.setPhone(patient.getUser().getPhone());
            dto.setGender(patient.getUser().getGender());
            dto.setDateOfBirth(patient.getUser().getDateOfBirth());
            dto.setAddress(patient.getUser().getAddress());
        }

        if (patient.getInsurance() != null) {
            dto.setInsuranceProvider(patient.getInsurance().getInsuranceProvider());
            dto.setPolicyNumber(patient.getInsurance().getPolicyNumber());
        }

        return dto;
    }
}
