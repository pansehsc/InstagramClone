using System;

namespace API.DTOs.Notifications;

public class NotificationDto
{
    public Guid Id { get; set; }
    public string Message { get; set; } = "";
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
