using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class PostRepository(AppDbContext context) : IPostRepository
{
    public void Add(Post post)
    {
        context.Posts.Add(post);
    }

    public void Delete(Post post)
    {
        context.Posts.Remove(post);
    }

    public void Update(Post post)
    {
        context.Entry(post).State = EntityState.Modified;
    }

    public async Task<Post?> GetByIdAsync(Guid id)
    {
        return await context.Posts
            .AsSplitQuery()
            .Include(p => p.User)
                .ThenInclude(u => u.Photos)
            .Include(p => p.Photos)
            .Include(p => p.Comments)
            .Include(p => p.Likes)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        return await context.Posts
            .AsSplitQuery()
            .Include(p => p.User)
                .ThenInclude(u => u.Photos)
            .Include(p => p.Photos)
            .Include(p => p.Comments)
            .Include(p => p.Likes)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetFeedAsync(Guid currentUserId)
    {
        return await context.Posts
            .AsSplitQuery()
            .Include(p => p.User)
                .ThenInclude(u => u.Photos)
            .Include(p => p.Photos)
            .Include(p => p.Likes)
            .Include(p => p.Comments)
            .Where(p =>
            p.UserId == currentUserId ||
            context.Follow.Any(f =>
            f.FollowerId == currentUserId &&
            f.FollowingId == p.UserId))
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

}