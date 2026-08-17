using API.Entities;

namespace API.Repositories;

public interface IStoryRepository
{
    Task<Story?> GetByIdAsync(Guid id);
    Task<IEnumerable<Story>> GetActiveStoriesAsync();
    Task<IEnumerable<Story>> GetUserStoriesAsync(Guid userId);
    void Add(Story story);
    void Delete(Story story);
    Task<bool> SaveAllAsync();
}