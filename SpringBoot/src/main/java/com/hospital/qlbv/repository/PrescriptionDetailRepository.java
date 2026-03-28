package com.hospital.qlbv.repository;

import com.hospital.qlbv.entity.PrescriptionDetail;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface PrescriptionDetailRepository extends JpaRepository<PrescriptionDetail, Integer> {
    List<PrescriptionDetail> findByPrescriptionId(Integer prescriptionId);
}
