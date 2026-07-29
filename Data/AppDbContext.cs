using System;
using API.Entities;
using Microsoft.EntityFrameworkCore;
namespace API.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Follow> Follow { get; set; }
    public DbSet<Photo> Photo { get; set; }
    public DbSet<Like> Like { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Messaging> Messaging { get; set; }
    public DbSet<Notification> Notification { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)

    {
        base.OnModelCreating(builder);
        
        // builder.Entity<k
        builder.Entity<Follow>()
        .HasOne(f => f.Follower)
        .WithMany(user => user.Following)
        .HasForeignKey(f => f.FollowerId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Follow>()
        .HasOne(f => f.Following)
        .WithMany(user => user.Followers)
        .HasForeignKey(f => f.FollowingId)
        .OnDelete(DeleteBehavior.Restrict);    
    }
}
// This is the connection between C# and SQL/SQLite.