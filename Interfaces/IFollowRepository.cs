using API.Entities;

namespace API.Interfaces;

public interface IFollowRepository
{
    Task<Follow?> GetFollowAsync(Guid followerId, Guid followingId);
    Task<IEnumerable<Follow>> GetFollowersAsync(Guid userId);
    Task<IEnumerable<Follow>> GetFollowingAsync(Guid userId);
    void Add(Follow follow);
    void Delete(Follow follow);
    Task<bool> SaveAllAsync();
    Task<bool> IsFollowingAsync(Guid currentUserId, Guid userId);
}