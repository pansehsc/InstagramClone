using System;

namespace API.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    //public required User CreatedBy { get; set; }
    public Guid CreatedById { get; set; }
    public string Content { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public Guid PostId { get; set; }
    //public Post Post { get; set; } = null!;
    public Photo? Photo { get; set; }
}
