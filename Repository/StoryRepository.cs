using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class StoryRepository(AppDbContext context)
    : IStoryRepository
{
    public async Task<Story?> GetByIdAsync(Guid id)
    {
        return await context.Stories
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Story>> GetActiveStoriesAsync()
    {
        return await context.Stories
            .Include(s => s.User)
            .Where(s => s.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Story>> GetUserStoriesAsync(
        Guid userId)
    {
        return await context.Stories
            .Include(s => s.User)
            .Where(s =>
                s.UserId == userId &&
                s.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public void Add(Story story)
    {
        context.Stories.Add(story);
    }

    public void Delete(Story story)
    {
        context.Stories.Remove(story);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}