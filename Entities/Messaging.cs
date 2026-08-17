using System;

namespace API.Entities;

public class Messaging
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Content { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } =false;
    public DateTime? ReadAt { get; set; }
    // Sender
    public Guid SenderId { get; set; }
    public User Sender { get; set; } = null!;
    // Receiver
    public Guid ReceiverId { get; set; }
    public User Receiver { get; set; } = null!;
    public bool SenderDeleted { get; set; } =false;
    public bool RecipientDeleted { get; set; } =false;
}