using System;

namespace API.Entities;

public class Post
{
    public Guid Id { get; set; }

    public string? Caption { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? Hashtags { get; set; }
    // Owner
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<Photo> Photos { get; set; } = [];

    public ICollection<Comment> Comments { get; set; } = [];

    public ICollection<Like> Likes { get; set; } = [];

}