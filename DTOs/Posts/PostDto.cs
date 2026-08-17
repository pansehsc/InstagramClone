using System;
using API.DTOs.Photos;

namespace API.DTOs.Posts;

public class PostDto
{
    public Guid Id { get; set; }
    public string? Caption { get; set; } 
    public string? Location { get; set; }
    public string? Hashtags { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UserName { get; set; } = "";
    public Guid UserId { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }
    public List<PhotoDto> Photos { get; set; } = [];
}
