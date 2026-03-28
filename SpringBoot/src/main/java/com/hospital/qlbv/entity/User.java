package com.hospital.qlbv.entity;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.*;
import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "users")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class User {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer userId;
    
    @Column(unique = true, nullable = false)
    private String username;
    
    @Column(nullable = false)
    private String passwordHash;
    
    @Column(unique = true)
    private String email;
    
    private String phone;
    private String fullName;
    private String gender;
    
    @Column(name = "date_of_birth")
    private String dateOfBirth;
    
    @Column(name = "created_at")
    private String createdAt;
    
    private String status;
    private String address;
    private String cccd;
    private String avatar;
    
    @OneToMany(mappedBy = "user", cascade = CascadeType.ALL, orphanRemoval = true)
    @JsonIgnoreProperties("user")
    @Builder.Default
    private List<UserRole> userRoles = new ArrayList<>();
    
    @OneToMany(mappedBy = "sender", cascade = CascadeType.ALL)
    @JsonIgnoreProperties("sender")
    @Builder.Default
    private List<ChatMessage> chatMessages = new ArrayList<>();
    
    @OneToMany(mappedBy = "user", cascade = CascadeType.ALL, orphanRemoval = true)
    @JsonIgnoreProperties("user")
    @Builder.Default
    private List<Notification> notifications = new ArrayList<>();
}
