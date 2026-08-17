using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class MessageRepository(AppDbContext context)
    : IMessageRepository
{
    public void Add(Messaging message)
    {
        context.Messages.Add(message);
    }



    public void Delete(Messaging message)
    {
        context.Messages.Remove(message);
    }

    public async Task<Messaging?> GetByIdAsync(Guid id)
    {
        return await context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .SingleOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Messaging>> GetConversationAsync(
        Guid currentUserId,
        Guid otherUserId)
    {
        return await context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m =>
                (
                    m.SenderId == currentUserId &&
                    m.ReceiverId == otherUserId &&
                    !m.SenderDeleted
                )
                ||
                (
                    m.SenderId == otherUserId &&
                    m.ReceiverId == currentUserId &&
                    !m.RecipientDeleted
                )
            )
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Messaging>> GetInboxAsync(
        Guid currentUserId)
    {
        return await context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m =>
                (m.SenderId == currentUserId && !m.SenderDeleted) ||
                (m.ReceiverId == currentUserId && !m.RecipientDeleted))
            .OrderByDescending(m => m.SentAt)
            .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }


}
