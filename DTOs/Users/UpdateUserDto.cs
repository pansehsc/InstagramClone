using System;

namespace API.DTOs.Users;

public class UpdateUserDto
{
    public string? UserName { get; set; } 
    public string? Bio { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}
