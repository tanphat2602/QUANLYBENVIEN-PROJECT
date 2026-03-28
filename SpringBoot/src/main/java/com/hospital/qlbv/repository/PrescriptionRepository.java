package com.hospital.qlbv.repository;

import com.hospital.qlbv.entity.Prescription;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface PrescriptionRepository extends JpaRepository<Prescription, Integer> {
    List<Prescription> findByRecordId(Integer recordId);
    List<Prescription> findByRecordIdIn(List<Integer> recordIds);
}
