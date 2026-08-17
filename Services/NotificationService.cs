using API.DTOs.Notifications;
using API.Entities;
using API.Interfaces;
using AutoMapper;

namespace API.Services;

public class NotificationService(
    INotificationRepository notificationRepository,
    IMapper mapper)
    : INotificationService
{
    public async Task CreateNotificationAsync(
        CreateNotificationDto dto)
    {
        
        var notification =
        mapper.Map<Notification>(dto);

    notificationRepository.Add(notification);

    await notificationRepository.SaveAllAsync();
    }
}