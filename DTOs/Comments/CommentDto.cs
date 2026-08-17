namespace API.DTOs.Comments;

public class CommentDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public Guid CreatedById { get; set; }
    public string UserName { get; set; } = "";
    public string? ProfilePictureUrl { get; set; }

}