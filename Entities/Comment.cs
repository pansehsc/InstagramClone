using System;

namespace API.Entities;

public class Comment
{   //post id in url
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    //public required User CreatedBy { get; set; }
    public Guid CreatedById { get; set; }
    public User User { get; set; } = null!;
    public string Content { get; set; } = string.Empty;
    //public string? ImageUrl { get; set; }
    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;
}
