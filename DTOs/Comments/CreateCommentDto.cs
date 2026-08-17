using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Comments;

public class CreateCommentDto
{
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = "";
}
