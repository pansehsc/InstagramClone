using System;

namespace API.DTOs.Users;

public class UserProfileDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = "";
    public string? Bio { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public int Followers { get; set; }
    public int Following { get; set; }
    public int Posts { get; set; }
}
