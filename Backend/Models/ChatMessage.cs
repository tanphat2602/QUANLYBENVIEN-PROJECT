using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class ChatMessage
{
    [Key]
    public int MessageID { get; set; }
    public int? ChatID { get; set; }
    public int? SenderID { get; set; }
    public string? MessageContent { get; set; }
    public DateTime? SentAt { get; set; }
    public bool? IsRead { get; set; }

    // Navigation properties
    public virtual Chat? Chat { get; set; }
    public virtual User? Sender { get; set; }
}
