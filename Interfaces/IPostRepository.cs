using API.Entities;

namespace API.Interfaces;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(Guid id);
    Task<IEnumerable<Post>> GetFeedAsync(Guid currentUserId);
    Task<IEnumerable<Post>> GetAllAsync();

    void Add(Post post);

    void Update(Post post);

    void Delete(Post post);

    Task<bool> SaveAllAsync();
}