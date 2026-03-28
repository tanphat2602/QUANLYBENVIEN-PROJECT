package com.hospital.qlbv.entity;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.*;
import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "medical_records")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class MedicalRecord {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer recordId;
    
    @Column(name = "patient_id")
    private Integer patientId;
    
    @Column(name = "doctor_id")
    private Integer doctorId;
    
    @Column(name = "appointment_id")
    private Integer appointmentId;
    
    private String symptoms;
    private String diagnosis;
    private String treatment;
    private String notes;
    private String status;
    
    @Column(name = "date_created")
    private String dateCreated;
    
    @Column(name = "updated_at")
    private String updatedAt;
    
    @Column(name = "vital_signs")
    private String vitalSigns;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "patient_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"user", "insurance"})
    private Patient patient;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "doctor_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"user", "department"})
    private Doctor doctor;
    
    @OneToMany(mappedBy = "medicalRecord", cascade = CascadeType.ALL)
    @JsonIgnoreProperties({"medicalRecord"})
    @Builder.Default
    private List<Prescription> prescriptions = new ArrayList<>();
    
    @OneToMany(mappedBy = "medicalRecord", cascade = CascadeType.ALL)
    @JsonIgnoreProperties({"medicalRecord"})
    @Builder.Default
    private List<TestRequest> testRequests = new ArrayList<>();
}
