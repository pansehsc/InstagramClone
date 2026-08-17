using API.Entities;
namespace API.Interfaces;

public interface INotificationRepository
{
    Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId);
    Task<Notification?> GetByIdAsync(Guid id);
    void Add(Notification notification);
    void Delete(Notification notification);
    Task<bool> SaveAllAsync();
}