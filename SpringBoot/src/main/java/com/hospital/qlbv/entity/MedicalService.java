package com.hospital.qlbv.entity;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "medical_services")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class MedicalService {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer serviceId;
    
    @Column(name = "service_name", nullable = false)
    private String serviceName;
    
    private String description;
    
    private Double price;
    
    @Column(name = "department_id")
    private Integer departmentId;
    
    private String status;
}
