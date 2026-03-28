package com.hospital.qlbv.entity;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "chats")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Chat {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer chatId;
    
    @Column(name = "sender_id")
    private Integer senderId;
    
    @Column(name = "receiver_id")
    private Integer receiverId;
    
    @Column(name = "created_at")
    private String createdAt;
    
    private String status;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "sender_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"passwordHash", "userRoles"})
    private User sender;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "receiver_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"passwordHash", "userRoles"})
    private User receiver;
}
