package com.hospital.qlbv.service;

import com.hospital.qlbv.dto.DoctorDTO;
import com.hospital.qlbv.entity.Doctor;
import com.hospital.qlbv.repository.DoctorRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.stream.Collectors;

@Service
public class DoctorService {

    private final DoctorRepository doctorRepository;

    public DoctorService(DoctorRepository doctorRepository) {
        this.doctorRepository = doctorRepository;
    }

    public List<DoctorDTO> getAllDoctors() {
        return doctorRepository.findAll().stream()
                .map(this::toDTO)
                .collect(Collectors.toList());
    }

    public DoctorDTO getDoctorById(Integer id) {
        Doctor doctor = doctorRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Doctor not found"));
        return toDTO(doctor);
    }

    @Transactional
    public DoctorDTO createDoctor(Doctor doctor) {
        doctor = doctorRepository.save(doctor);
        return toDTO(doctor);
    }

    @Transactional
    public DoctorDTO updateDoctor(Integer id, Doctor doctorData) {
        Doctor doctor = doctorRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Doctor not found"));
        
        if (doctorData.getSpecialty() != null) doctor.setSpecialty(doctorData.getSpecialty());
        if (doctorData.getWorkingSchedule() != null) doctor.setWorkingSchedule(doctorData.getWorkingSchedule());
        if (doctorData.getDepartmentId() != null) doctor.setDepartmentId(doctorData.getDepartmentId());
        if (doctorData.getStatus() != null) doctor.setStatus(doctorData.getStatus());
        
        doctor = doctorRepository.save(doctor);
        return toDTO(doctor);
    }

    @Transactional
    public void deleteDoctor(Integer id) {
        doctorRepository.deleteById(id);
    }

    private DoctorDTO toDTO(Doctor doctor) {
        DoctorDTO dto = DoctorDTO.builder()
                .doctorId(doctor.getDoctorId())
                .userId(doctor.getUserId())
                .specialty(doctor.getSpecialty())
                .workingSchedule(doctor.getWorkingSchedule())
                .experience(doctor.getExperience())
                .education(doctor.getEducation())
                .consultationFee(doctor.getConsultationFee())
                .departmentId(doctor.getDepartmentId())
                .status(doctor.getStatus())
                .build();

        if (doctor.getUser() != null) {
            dto.setFullName(doctor.getUser().getFullName());
            dto.setEmail(doctor.getUser().getEmail());
            dto.setPhone(doctor.getUser().getPhone());
        }

        if (doctor.getDepartment() != null) {
            dto.setDepartmentName(doctor.getDepartment().getDepartmentName());
        }

        return dto;
    }
}
