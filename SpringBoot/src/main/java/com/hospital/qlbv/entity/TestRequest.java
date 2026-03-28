package com.hospital.qlbv.entity;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "test_requests")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class TestRequest {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer testId;
    
    @Column(name = "record_id")
    private Integer recordId;
    
    @Column(name = "patient_id")
    private Integer patientId;
    
    @Column(name = "test_type")
    private String testType;
    
    private String description;
    
    @Column(name = "request_date")
    private String requestDate;
    
    @Column(name = "result_date")
    private String resultDate;
    
    private String result;
    private String status;
    private Double fee;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "record_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"patient", "doctor", "prescriptions", "testRequests"})
    private MedicalRecord medicalRecord;
}
