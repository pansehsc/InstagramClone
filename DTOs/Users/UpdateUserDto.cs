using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Users;

public class UpdateUserDto
{
    public string? UserName { get; set; } 
    [MaxLength(200)]
    public string? Bio { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? ProfilePictureUrl { get; set; }

}
