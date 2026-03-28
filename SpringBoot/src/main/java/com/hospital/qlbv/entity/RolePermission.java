package com.hospital.qlbv.entity;

import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "role_permissions")
@IdClass(RolePermissionId.class)
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class RolePermission {
    
    @Id
    @Column(name = "role_id")
    private Integer roleId;
    
    @Id
    @Column(name = "permission_id")
    private Integer permissionId;
}
