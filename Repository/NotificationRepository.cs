using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class NotificationRepository(AppDbContext context)
    : INotificationRepository
{
    public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId)
    {
        return await context.Notification
            .Include(n => n.Actor)
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task<Notification?> GetByIdAsync(Guid id)
    {
        return await context.Notification
            .Include(n => n.Actor)
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public void Add(Notification notification)
    {
        context.Notification.Add(notification);
    }

    public void Delete(Notification notification)
    {
        context.Notification.Remove(notification);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}