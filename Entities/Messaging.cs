using System;

namespace API.Entities;

public class Messaging
{
    public Guid Id { get; set; }
    public required string Content { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }//
    public DateTime? ReadAt { get; set; }
    // Sender
    public Guid SenderId { get; set; }
    public User Sender { get; set; } = null!;
    // Receiver
    public Guid ReceiverId { get; set; }
    public User Receiver { get; set; } = null!;
    public bool SenderDeleted { get; set; }
    public bool RecipientDeleted { get; set; }
}