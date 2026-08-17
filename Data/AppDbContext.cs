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
    public DbSet<Messaging> Messages { get; set; }
    public DbSet<Notification> Notification { get; set; }
    public DbSet<Story> Stories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)

    { // set relations 
        base.OnModelCreating(builder);
        builder.Entity<Follow>()
            .HasKey(f => new { f.FollowerId, f.FollowingId });

        builder.Entity<Follow>()
            .HasOne(f => f.Follower)
            .WithMany(user => user.Followers)
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Follow>()
            .HasOne(f => f.Following)
            .WithMany(user => user.Following)
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

        builder.Entity<Like>() //Composite key.
            .HasKey(x => new { x.CreatedById, x.PostId }); //

        builder.Entity<Like>()
            .HasOne(x => x.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(x => x.CreatedById);

        builder.Entity<Like>()
            .HasOne(x => x.Post)
            .WithMany(p => p.Likes)
            .HasForeignKey(x => x.PostId);

        builder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Notification>()
            .HasOne(n => n.Actor)
            .WithMany()
            .HasForeignKey(n => n.ActorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Photo>()
            .HasOne(p => p.User)
            .WithMany(u => u.Photos)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Photo>()
            .HasOne(p => p.Post)
            .WithMany(p => p.Photos)
            .HasForeignKey(p => p.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
// This is the connection between C# and SQL/SQLite.