using Microsoft.AspNetCore.Http;

namespace API.DTOs.Stories;

public class CreateStoryDto
{
    public string? Content { get; set; }
    public IFormFile? File { get; set; }
}
