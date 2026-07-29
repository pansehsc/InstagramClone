using System;

namespace API.Entities;

public class Post
{
    public Guid Id { get; set; }
    public string? Caption { get; set; }
    //public string? ImageUrl { get; set; }
    public Guid UserId { get; set; }
    public required User User {get; set;}
    public ICollection<Comment> Comments {get;set;}=[];
    public ICollection<Photo> Photos { get; set;}=[];
    public ICollection<Like> Like { get; set;}=[];

}
