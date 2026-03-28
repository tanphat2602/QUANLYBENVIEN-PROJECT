package com.hospital.qlbv.entity;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "prescription_details")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class PrescriptionDetail {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer detailId;
    
    @Column(name = "prescription_id")
    private Integer prescriptionId;
    
    @Column(name = "medicine_id")
    private Integer medicineId;
    
    private String dosage;
    private String quantity;
    
    @Column(name = "usage_instructions")
    private String usageInstructions;
    
    private Integer duration;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "prescription_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"details"})
    private Prescription prescription;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "medicine_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"prescriptionDetails"})
    private Medicine medicine;
}
