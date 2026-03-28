package com.hospital.qlbv.repository;

import com.hospital.qlbv.entity.Patient;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import java.util.Optional;

@Repository
public interface PatientRepository extends JpaRepository<Patient, Integer> {
    
    Optional<Patient> findByUserId(Integer userId);
    
    @Query("SELECT COUNT(p) FROM Patient p")
    long countPatients();
}
