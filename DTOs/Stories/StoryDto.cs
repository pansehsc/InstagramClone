namespace API.DTOs.Stories;

public class StoryDto
{
    public Guid Id { get; set; }
    public string? Url { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = "";
    public string? ProfilePictureUrl { get; set; }
}

