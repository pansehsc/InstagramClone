using API.Entities;

namespace API.Interfaces;

public interface ILikeRepository
{
    Task<Like?> GetLikeAsync(Guid userId, Guid postId);
    Task AddAsync(Like like);
    void Delete(Like like);
    Task<bool> SaveAllAsync();
}
