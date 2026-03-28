package com.hospital.qlbv.entity;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "notifications")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Notification {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer notificationId;
    
    @Column(name = "user_id")
    private Integer userId;
    
    private String title;
    private String content;
    private String type;
    
    @Column(name = "is_read")
    private Boolean isRead;
    
    @Column(name = "created_at")
    private String createdAt;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "user_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"passwordHash", "userRoles", "email", "phone", "fullName"})
    private User user;
}
