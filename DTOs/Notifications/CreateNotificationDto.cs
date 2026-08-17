namespace API.DTOs.Notifications;

public class CreateNotificationDto
{
    public Guid UserId { get; set; }
    public Guid ActorId { get; set; }
    public string NotificationType { get; set; } = string.Empty;
    public Guid? PostId { get; set; }
}
