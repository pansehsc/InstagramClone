using System;

namespace API.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpires { get; set; }
    public required string Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public required string Country { get; set; }
    public required string City { get; set; }
    // public string? ProfilePictureUrl { get; set; }
    // public string? ProfilePicturePublicId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow; 
    public string? Bio { get; set; }
    // Navigation properties
    public ICollection<Post> Posts { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<Photo> Photos { get; set; } = [];
    public ICollection<Messaging> SentMessages { get; set; } = [];
    public ICollection<Messaging> ReceivedMessages { get; set; } = [];
    public ICollection<Like> Likes { get; set; } = [];
    // users who follow me
    public ICollection<Follow> Followers { get; set; } = [];
    // users i follow
    public ICollection<Follow> Following { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];
}
