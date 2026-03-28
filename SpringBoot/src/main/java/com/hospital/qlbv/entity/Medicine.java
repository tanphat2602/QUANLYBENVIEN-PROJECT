package com.hospital.qlbv.entity;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.*;
import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "medicines")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Medicine {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer medicineId;
    
    @Column(name = "medicine_name", nullable = false)
    private String medicineName;
    
    @Column(name = "generic_name")
    private String genericName;
    
    private String manufacturer;
    private String unit;
    
    @Column(precision = 10, scale = 2)
    private BigDecimal price;
    
    private Integer stock;
    
    @Column(name = "dosage_form")
    private String dosageForm;
    
    @Column(name = "side_effects")
    private String sideEffects;
    
    @Column(name = "contraindications")
    private String contraindications;
    
    @Column(name = "expiry_date")
    private String expiryDate;
    
    private String status;
    
    @OneToMany(mappedBy = "medicine", cascade = CascadeType.ALL)
    @JsonIgnoreProperties("medicine")
    @Builder.Default
    private List<PrescriptionDetail> prescriptionDetails = new ArrayList<>();
}
