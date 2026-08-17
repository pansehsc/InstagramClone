using System;
using System.Threading.Tasks;
using API.DTOs.Notifications;

namespace API.Interfaces;

public interface INotificationService
{
    Task CreateNotificationAsync(CreateNotificationDto dto);
}