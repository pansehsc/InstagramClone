using System;

namespace API.DTOs.Notifications;

public class NotificationDto
{
    public Guid Id { get; set; }
    public Guid ActorId { get; set; }
    public string ActorUserName { get; set; } = "";
    public string? ActorProfilePictureUrl { get; set; }
    public string Type { get; set; } = "";
    public Guid? PostId { get; set; }
    public Guid? CommentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
}
