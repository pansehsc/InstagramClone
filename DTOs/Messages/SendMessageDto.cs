using System;

namespace API.DTOs.Messages;

public class SendMessageDto
{
    public Guid ReceiverId { get; set; }
    public string Content { get; set; } = "";
}
