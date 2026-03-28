package com.hospital.qlbv.dto;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;
import java.math.BigDecimal;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class DoctorDTO {
    private Integer doctorId;
    private Integer userId;
    private String fullName;
    private String email;
    private String phone;
    private String specialty;
    private String workingSchedule;
    private String experience;
    private String education;
    private BigDecimal consultationFee;
    private String departmentName;
    private Integer departmentId;
    private String status;
}
