package com.hospital.qlbv.entity;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "payments")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Payment {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer paymentId;
    
    @Column(name = "patient_id")
    private Integer patientId;
    
    @Column(name = "appointment_id")
    private Integer appointmentId;
    
    private Double amount;
    
    @Column(name = "payment_method")
    private String paymentMethod;
    
    private String status;
    
    @Column(name = "transaction_id")
    private String transactionId;
    
    @Column(name = "payment_date")
    private String paymentDate;
    
    private String description;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "patient_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"user", "insurance", "appointments"})
    private Patient patient;
}
