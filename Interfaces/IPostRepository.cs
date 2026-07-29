using API.Entities;

namespace API.Interfaces;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(Guid id);
    Task<IEnumerable<Post>> GetFeedAsync();
    Task<IEnumerable<Post>> GetUserPostsAsync(Guid userId);
    Task AddAsync(Post post);
    void Delete(Post post);
    Task<bool> SaveAllAsync();
}

