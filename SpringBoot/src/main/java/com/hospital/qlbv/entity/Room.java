package com.hospital.qlbv.entity;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.*;
import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "rooms")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Room {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer roomId;
    
    @Column(name = "room_number", nullable = false)
    private String roomNumber;
    
    @Column(name = "department_id")
    private Integer departmentId;
    
    @Column(name = "room_type")
    private String roomType;
    
    private String status;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "department_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"doctors", "rooms"})
    private Department department;
    
    @OneToMany(mappedBy = "room", cascade = CascadeType.ALL)
    @JsonIgnoreProperties({"room", "user", "department"})
    @Builder.Default
    private List<Doctor> doctors = new ArrayList<>();
}
