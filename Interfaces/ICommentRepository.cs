using API.Entities;

namespace API.Interfaces;

public interface ICommentRepository
{
    Task<Comment?> GetByIdAsync(Guid id);
    Task<IEnumerable<Comment>> GetByPostIdAsync(Guid postId);
    void Add(Comment comment);
    void Delete(Comment comment);
    Task<bool> SaveAllAsync();
}


