using System;

namespace API.DTOs.Messages;

public class MessageDto
{
    public Guid Id { get; set; }
    public string SenderUserName { get; set; } = "";
    public string ReceiverUserName { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
}
