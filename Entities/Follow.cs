using System;

namespace API.Entities;

public class Follow
{
    // public Guid Id { get; set; }
    public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
    // The user who follows
    public Guid FollowerId { get; set; }
    public required User Follower { get; set; } 
    // The user being followed
    public Guid FollowingId { get; set; }
    public required User Following { get; set; } 
}
