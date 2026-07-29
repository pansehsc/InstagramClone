using API.Entities;

namespace API.Interfaces;

public interface ICommentRepository
{
    Task<Comment?> GetByIdAsync(Guid id);
    Task<IEnumerable<Comment>> GetPostCommentsAsync(Guid postId);
    Task AddAsync(Comment comment);
    void Delete(Comment comment);
    Task<bool> SaveAllAsync();
}


