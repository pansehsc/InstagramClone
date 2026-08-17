using API.Entities;

namespace API.Interfaces;

public interface IMessageRepository
{
    Task<Messaging?> GetByIdAsync(Guid id);
    Task<IEnumerable<Messaging>> GetConversationAsync(Guid currentUserId, Guid otherUserId);
    Task<IEnumerable<Messaging>> GetInboxAsync(Guid currentUserId);
    void Add(Messaging message);
    void Delete(Messaging message);
    Task<bool> SaveAllAsync();
}