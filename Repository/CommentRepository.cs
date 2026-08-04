using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class CommentRepository(AppDbContext context) : ICommentRepository
{
    public async Task<Comment?> GetByIdAsync(Guid id)
    {
        return await context.Comments
            .Include(c => c.User)
            .Include(c => c.Post)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task<IEnumerable<Comment>> GetByPostIdAsync(Guid postId)
    {
        return await context.Comments
            .Include(c => c.User)
            .Where(c => c.PostId == postId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }
    public void Add(Comment comment)
    {
        context.Comments.Add(comment);
    }
    public void Delete(Comment comment)
    {
        context.Comments.Remove(comment);
    }
    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

}