using API.Entities;

public class Photo
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? PublicId { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public Guid? PostId { get; set; }
    public Post? Post { get; set; }
    public bool IsMain { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    //public Post Post { get; set; } = null!;
}