package com.hospital.qlbv.repository;

import com.hospital.qlbv.entity.EmergencyRequest;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface EmergencyRequestRepository extends JpaRepository<EmergencyRequest, Integer> {
}
