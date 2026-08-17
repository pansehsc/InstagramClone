using API.Entities;

public class Story
{
    public Guid Id { get; set; }
    // cloudinary url
    public string? Url { get; set; }
    public string? PublicId { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

}
