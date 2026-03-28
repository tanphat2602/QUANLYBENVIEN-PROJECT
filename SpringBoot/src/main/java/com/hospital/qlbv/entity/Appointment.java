package com.hospital.qlbv.entity;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.*;
import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "appointments")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Appointment {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer appointmentId;
    
    @Column(name = "patient_id")
    private Integer patientId;
    
    @Column(name = "doctor_id")
    private Integer doctorId;
    
    @Column(name = "department_id")
    private Integer departmentId;
    
    @Column(name = "appointment_date")
    private String appointmentDate;
    
    @Column(name = "time_slot")
    private String timeSlot;
    
    private String status;
    private String notes;
    private String reason;
    
    @Column(name = "appointment_type")
    private String appointmentType;
    
    @Column(name = "is_emergency")
    private Boolean isEmergency;
    
    private Integer priority;
    
    @Column(name = "created_at")
    private String createdAt;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "patient_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"user", "insurance", "appointments"})
    private Patient patient;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "doctor_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"user", "department", "room", "appointments"})
    private Doctor doctor;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "department_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"doctors", "rooms"})
    private Department department;
}
