using System;
using API.DTOs.Posts;
namespace API.DTOs.Users;

public class UserProfileDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = "";
    public string? Bio { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
    public int PostsCount { get; set; }
    public List<PostDto> Posts { get; set; } = [];

}
