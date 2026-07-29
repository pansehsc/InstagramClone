using System;

namespace API.DTOs.Posts;

public class PostDto
{
    public Guid Id { get; set; }
    public string Caption { get; set; } = "";
    public string UserName { get; set; } = "";
    public string? ProfilePictureUrl { get; set; }
    public string? ImageUrl { get; set; }
    public int Likes { get; set; }
    public int Comments { get; set; }
}
