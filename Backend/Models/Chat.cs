using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class Chat
{
    [Key]
    public int ChatID { get; set; }
    public int? SenderID { get; set; }
    public int? ReceiverID { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Status { get; set; }

    // Navigation properties
    public virtual User? Sender { get; set; }
    public virtual User? Receiver { get; set; }
    public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}
