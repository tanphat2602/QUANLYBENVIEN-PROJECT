package com.hospital.qlbv.entity;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.*;
import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "prescriptions")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Prescription {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer prescriptionId;
    
    @Column(name = "record_id")
    private Integer recordId;
    
    private String diagnosis;
    
    @Column(name = "created_at")
    private String createdAt;
    
    private String status;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "record_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"prescriptions", "testRequests"})
    private MedicalRecord medicalRecord;
    
    @OneToMany(mappedBy = "prescription", cascade = CascadeType.ALL)
    @JsonIgnoreProperties({"prescription"})
    @Builder.Default
    private List<PrescriptionDetail> details = new ArrayList<>();
}
