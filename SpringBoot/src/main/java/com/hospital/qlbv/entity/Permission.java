package com.hospital.qlbv.entity;

import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "permissions")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Permission {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer permissionId;
    
    @Column(unique = true, nullable = false)
    private String permissionName;
    
    private String description;
    
    private String resource;
    
    private String action;
}
