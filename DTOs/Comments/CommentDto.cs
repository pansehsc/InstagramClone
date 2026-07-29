using System;

namespace API.DTOs;

public class CommentDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
