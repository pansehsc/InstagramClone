using System;

namespace API.DTOs.Photos;

public class PhotoDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = "";
}
