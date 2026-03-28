package com.hospital.qlbv.dto;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;
import java.util.List;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class AuthResponse {
    private String token;
    private Integer userId;
    private String username;
    private String fullName;
    private String role;
    private String email;
    private Integer patientId;
    private Integer doctorId;
    private List<String> roles;
    private List<String> permissions;
}
