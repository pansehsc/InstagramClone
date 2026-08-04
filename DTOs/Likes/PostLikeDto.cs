using System;

namespace API.DTOs.Likes;

public class PostLikeDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = "";
    public string? ProfilePictureUrl { get; set; }
}
