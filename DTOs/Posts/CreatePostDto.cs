using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Posts;

public class CreatePostDto
{
    [MaxLength(3000)]
    public string? Caption { get; set; } = "";
    public List<IFormFile> Photos { get; set; } = [];
    public string? Location { get; set; } = "";
    public string? Hashtags { get; set; }
}
