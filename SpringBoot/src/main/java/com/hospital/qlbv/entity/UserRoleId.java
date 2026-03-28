package com.hospital.qlbv.entity;

import jakarta.persistence.*;
import lombok.*;
import java.io.Serializable;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class UserRoleId implements Serializable {
    private Integer userId;
    private Integer roleId;
}
