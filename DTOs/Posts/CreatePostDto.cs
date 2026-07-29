using System;

namespace API.DTOs.Posts;

public class CreatePostDto
{
    public string? Caption { get; set; } = "";
    public string? ImageUrl { get; set; } = "";

}
