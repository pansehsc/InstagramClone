using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class LikeRepository(AppDbContext context) : ILikeRepository
{
    public async Task<Like?> GetLikeAsync(Guid userId, Guid postId)
    {
        return await context.Like
            .FirstOrDefaultAsync(x =>
                x.CreatedById == userId &&
                x.PostId == postId);
    }

    public async Task<int> GetLikesCountAsync(Guid postId)
    {
        return await context.Like
            .CountAsync(x => x.PostId == postId);
    }

    public async Task<IEnumerable<Like>> GetPostLikesAsync(Guid postId)
    {
        return await context.Like
            .Include(x => x.User)
            .Where(x => x.PostId == postId)
            .ToListAsync();
    }

    public void Add(Like like)
    {
        context.Like.Add(like);
    }

    public void Delete(Like like)
    {
        context.Like.Remove(like);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}