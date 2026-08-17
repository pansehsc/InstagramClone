using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class FollowRepository(AppDbContext context) : IFollowRepository
{
    public async Task<Follow?> GetFollowAsync(Guid followerId, Guid followingId)
    {
        return await context.Follow
            .FirstOrDefaultAsync(f =>
                f.FollowerId == followerId &&
                f.FollowingId == followingId);
    }

    public async Task<IEnumerable<Follow>> GetFollowersAsync(Guid userId)
    {
        return await context.Follow
            .Include(f => f.Follower)
            .Where(f => f.FollowingId == userId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Follow>> GetFollowingAsync(Guid userId)
    {
        return await context.Follow
            .Include(f => f.Following)
            .Where(f => f.FollowerId == userId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }

    public void Add(Follow follow)
    {
        context.Follow.Add(follow);
    }

    public void Delete(Follow follow)
    {
        context.Follow.Remove(follow);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
    public async Task<bool> IsFollowingAsync(
    Guid followerId,
    Guid followingId)
    {
        return await context.Follow
            .AnyAsync(f =>
                f.FollowerId == followerId &&
                f.FollowingId == followingId);
    }
}