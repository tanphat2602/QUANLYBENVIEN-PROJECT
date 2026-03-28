package com.hospital.qlbv.entity;

import jakarta.persistence.*;
import lombok.*;
import com.fasterxml.jackson.annotation.JsonIgnoreProperties;

@Entity
@Table(name = "user_roles")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
@IdClass(UserRoleId.class)
public class UserRole {
    
    @Id
    @Column(name = "user_id")
    private Integer userId;
    
    @Id
    @Column(name = "role_id")
    private Integer roleId;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "user_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"userRoles", "passwordHash", "email", "phone", "fullName"})
    private User user;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "role_id", insertable = false, updatable = false)
    @JsonIgnoreProperties("userRoles")
    private Role role;
}
