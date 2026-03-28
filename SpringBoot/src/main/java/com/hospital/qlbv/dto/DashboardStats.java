package com.hospital.qlbv.dto;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class DashboardStats {
    private long totalUsers;
    private long totalDoctors;
    private long totalPatients;
    private long totalAppointments;
    private Double totalRevenue;
    private long totalDepartments;
    private long todayAppointments;
    private long emergencyRequests;
    private long doctorsOnDuty;
}
