package com.hospital.qlbv.entity;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "emergency_requests")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class EmergencyRequest {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer emergencyId;
    
    @Column(name = "patient_id")
    private Integer patientId;
    
    @Column(name = "assigned_doctor_id")
    private Integer assignedDoctorId;
    
    private String reason;
    private String symptoms;
    private String status;
    private String priority;
    
    @Column(name = "requested_at")
    private String requestedAt;
    
    @Column(name = "assigned_at")
    private String assignedAt;
    
    @Column(name = "completed_at")
    private String completedAt;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "patient_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"user", "insurance"})
    private Patient patient;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "assigned_doctor_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"user", "department"})
    private Doctor assignedDoctor;
}
