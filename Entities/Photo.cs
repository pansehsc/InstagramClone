using API.Entities;

public class Photo
{
    public Guid Id { get; set; } = Guid.NewGuid(); // Database ID
    public string Url { get; set; } = string.Empty;
    public string? PublicId { get; set; } // Cloudinary ID
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    // Profile photo relationship
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    // Post photo relationship
    public Guid? PostId { get; set; }
    public Post? Post { get; set; }
    public bool IsMain { get; set; } // post main cover photo
}
