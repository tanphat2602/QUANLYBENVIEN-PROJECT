package com.hospital.qlbv.entity;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "doctor_schedules")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class DoctorSchedule {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer scheduleId;
    
    @Column(name = "doctor_id")
    private Integer doctorId;
    
    @Column(name = "day_of_week")
    private String dayOfWeek;
    
    private String shift;
    
    @Column(name = "time_start")
    private String timeStart;
    
    @Column(name = "time_end")
    private String timeEnd;
    
    @Column(name = "max_patients")
    private Integer maxPatients;
    
    @Column(name = "current_patients")
    private Integer currentPatients;
    
    private String status;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "doctor_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"user", "department", "room", "schedules"})
    private Doctor doctor;
}
