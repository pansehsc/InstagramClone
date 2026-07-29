using API.Entities;

namespace API.Interfaces;

public interface IFollowRepository
{
    Task<Follow?> GetFollowAsync(Guid followerId, Guid followingId);
    Task<IEnumerable<Follow>> GetFollowersAsync(Guid userId);
    Task<IEnumerable<Follow>> GetFollowingAsync(Guid userId);
    Task AddAsync(Follow follow);
    void Delete(Follow follow);
    Task<bool> SaveAllAsync();
}

