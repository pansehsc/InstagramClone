using System;
using API.Entities;
using Microsoft.EntityFrameworkCore;
namespace API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
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
        builder.Entity<Follow>()
        .HasKey(f => new { f.FollowerId, f.FollowingId });
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
        builder.Entity<Messaging>()
        .HasOne(m => m.Sender)
        .WithMany(u => u.SentMessages)
        .HasForeignKey(m => m.SenderId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Messaging>()
        .HasOne(m => m.Receiver)
        .WithMany(u => u.ReceivedMessages)
        .HasForeignKey(m => m.ReceiverId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Like>()
            .HasKey(x => new { x.CreatedById, x.PostId });

        builder.Entity<Like>()
            .HasOne(x => x.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(x => x.CreatedById);

        builder.Entity<Like>()
            .HasOne(x => x.Post)
            .WithMany(p => p.Likes)
            .HasForeignKey(x => x.PostId);

        builder.Entity<Follow>()
            .HasKey(x => new { x.FollowerId, x.FollowingId });

        builder.Entity<Follow>()
            .HasOne(x => x.Follower)
            .WithMany(x => x.Following)
            .HasForeignKey(x => x.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Follow>()
            .HasOne(x => x.Following)
            .WithMany(x => x.Followers)
            .HasForeignKey(x => x.FollowingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
// This is the connection between C# and SQL/SQLite.