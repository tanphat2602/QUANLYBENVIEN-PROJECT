package com.hospital.qlbv.dto;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class PatientDTO {
    private Integer patientId;
    private Integer userId;
    private String fullName;
    private String email;
    private String phone;
    private String gender;
    private String dateOfBirth;
    private String address;
    private String bloodType;
    private String allergies;
    private Double height;
    private Double weight;
    private String status;
    private String insuranceProvider;
    private String policyNumber;
    private String createdAt;
}
