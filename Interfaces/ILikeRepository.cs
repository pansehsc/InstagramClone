using API.Entities;

namespace API.Interfaces;

public interface ILikeRepository
{
    Task<Like?> GetLikeAsync(Guid userId, Guid postId);
    Task<int> GetLikesCountAsync(Guid postId);
    Task<IEnumerable<Like>> GetPostLikesAsync(Guid postId);
    void Add(Like like);
    void Delete(Like like);
    Task<bool> SaveAllAsync();
}