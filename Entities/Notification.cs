using System;

namespace API.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public enum NotificationType
    { Like, Comment, Follow, Message }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    // User who receives the notification
    public Guid UserId { get; set; }
    //public User User { get; set; } = null!;
    // User who triggered it
    public Guid TriggeredByUserId { get; set; }
    //public User? TriggeredByUser { get; set; }
    // Optional related post
    public Guid PostId { get; set; }
    //public Post Post { get; set; }
}
