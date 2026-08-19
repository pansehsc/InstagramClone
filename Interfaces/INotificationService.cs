using System;
using System.Threading.Tasks;
using API.DTOs.Notifications;
using API.Entities;

namespace API.Interfaces;

public interface INotificationService
{
    Task<NotificationDto?> CreateNotificationAsync(CreateNotificationDto dto);
}