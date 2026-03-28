package com.hospital.qlbv.repository;

import com.hospital.qlbv.entity.Doctor;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import java.util.Optional;

@Repository
public interface DoctorRepository extends JpaRepository<Doctor, Integer> {
    
    Optional<Doctor> findByUserId(Integer userId);
    
    @Query("SELECT COUNT(d) FROM Doctor d")
    long countDoctors();
}
