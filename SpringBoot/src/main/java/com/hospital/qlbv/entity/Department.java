package com.hospital.qlbv.entity;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.*;
import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "departments")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Department {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer departmentId;
    
    @Column(name = "department_name", nullable = false)
    private String departmentName;
    
    private String description;
    private String location;
    private String status;
    
    @OneToMany(mappedBy = "department", cascade = CascadeType.ALL)
    @JsonIgnoreProperties({"department", "doctors"})
    @Builder.Default
    private List<Doctor> doctors = new ArrayList<>();
    
    @OneToMany(mappedBy = "department", cascade = CascadeType.ALL)
    @JsonIgnoreProperties({"department", "doctors"})
    @Builder.Default
    private List<Room> rooms = new ArrayList<>();
}
