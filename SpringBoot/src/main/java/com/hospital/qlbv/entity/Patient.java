package com.hospital.qlbv.entity;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.*;
import java.math.BigDecimal;

@Entity
@Table(name = "patients")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Patient {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer patientId;
    
    @Column(name = "user_id")
    private Integer userId;
    
    @Column(name = "created_at")
    private String createdAt;
    
    private String status;
    private String bloodType;
    private String allergies;
    private BigDecimal height;
    private BigDecimal weight;
    
    @Column(name = "insurance_id")
    private Integer insuranceId;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "user_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"passwordHash", "userRoles", "email", "phone", "fullName"})
    private User user;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "insurance_id", insertable = false, updatable = false)
    @JsonIgnoreProperties("patients")
    private Insurance insurance;
}
