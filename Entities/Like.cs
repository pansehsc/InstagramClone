using System;

namespace API.Entities;

public class Like
{
    //public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public required Guid CreatedById { get; set; }
    public User User { get; set; } = null!;
}
