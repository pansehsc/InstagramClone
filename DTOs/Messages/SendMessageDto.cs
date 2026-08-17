using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Messages;

public class SendMessageDto
{
    [Required]
    public Guid ReceiverId { get; set; }
    [Required]
    [MaxLength(3000)]
    public string Content { get; set; } = "";
}
