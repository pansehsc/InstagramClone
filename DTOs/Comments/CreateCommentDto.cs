using System;

namespace API.DTOs.Comments;

public class CreateCommentDto
{
    public Guid PostId { get; set; }
    public string Content { get; set; } = "";
}
