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
public class UserDTO {
    private Integer userId;
    private String username;
    private String email;
    private String fullName;
    private String phone;
    private String gender;
    private String dateOfBirth;
    private String status;
    private String createdAt;
    private List<String> roles;
}
