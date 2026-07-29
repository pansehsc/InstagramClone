using API.Entities;

namespace API.Interfaces;

public interface IMessageRepository
{
    Task<Messaging?> GetByIdAsync(Guid id);
    Task<IEnumerable<Messaging>> GetConversationAsync(Guid senderId, Guid receiverId);
    Task AddAsync(Messaging message);
    void Delete(Messaging message);
    Task<bool> SaveAllAsync();
}