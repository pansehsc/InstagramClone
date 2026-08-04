using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Posts;

public class CreatePostDto
{
    [MaxLength(2000)]
    public string? Caption { get; set; } = "";
    public string? ImageUrl { get; set; } = "";
    public string? Location { get; set; } = "";
    public string? Hashtags { get; set; }
}
