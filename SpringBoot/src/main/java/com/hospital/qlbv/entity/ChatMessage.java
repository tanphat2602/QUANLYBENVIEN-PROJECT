package com.hospital.qlbv.entity;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "chat_messages")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class ChatMessage {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer messageId;
    
    @Column(name = "chat_id")
    private Integer chatId;
    
    @Column(name = "sender_id")
    private Integer senderId;
    
    @Column(name = "message_content")
    private String messageContent;
    
    @Column(name = "sent_at")
    private String sentAt;
    
    @Column(name = "is_read")
    private Boolean isRead;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "chat_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"messages", "sender", "receiver"})
    private Chat chat;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "sender_id", insertable = false, updatable = false)
    @JsonIgnoreProperties({"passwordHash", "userRoles", "chatMessages"})
    private User sender;
}
