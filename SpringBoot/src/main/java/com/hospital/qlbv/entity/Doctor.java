package com.hospital.qlbv.entity;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.*;
import java.math.BigDecimal;

@Entity
@Table(name = "doctors")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Doctor {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer doctorId;
    
    @Column(name = "user_id")
    private Integer userId;
    
    @Column(name = "department_id")
    private Integer departmentId;
    
    private String specialty;
    
    @Column(name = "working_schedule")
    private String workingSchedule;
    
    private String experience;
    private String education;
    
    @Column(name = "consultation_fee")
    private BigDecimal consultationFee;
    
    private String status;
    
    @Column(name = "room_id")
    private Integer roomId;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "user_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"passwordHash", "userRoles", "email", "phone", "fullName"})
    private User user;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "department_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"doctors", "rooms"})
    private Department department;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "room_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"doctors", "department"})
    private Room room;
}
