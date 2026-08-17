namespace API.DTOs.Follows;

public class FollowingDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = "";
    public string? ProfilePictureUrl { get; set; }
}