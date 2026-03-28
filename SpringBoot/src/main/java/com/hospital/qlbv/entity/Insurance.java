package com.hospital.qlbv.entity;

import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "insurance")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Insurance {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer insuranceId;
    
    @Column(name = "insurance_provider")
    private String insuranceProvider;
    
    @Column(name = "policy_number")
    private String policyNumber;
    
    @Column(name = "expiry_date")
    private String expiryDate;
    
    @Column(name = "coverage_amount")
    private Double coverageAmount;
}
