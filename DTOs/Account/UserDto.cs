using System;
//The body that returns to the front...
namespace API.DTOs.Account;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Token { get; set; } = "";
    public string? ProfilePictureUrl { get; set; }
    
}
