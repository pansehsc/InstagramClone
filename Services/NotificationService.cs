using API.Data;
using API.DTOs.Notifications;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class NotificationService(
    AppDbContext _context) : INotificationService
{
    public async Task<NotificationDto?> CreateNotificationAsync(CreateNotificationDto dto)
    {
        var actor = await _context.Users
            .Include(u => u.Photos) 
            .FirstOrDefaultAsync(u => u.Id == dto.ActorId);

        if (actor == null)
            return null;

        var notification = new Notification
        {
            UserId = dto.UserId,
            ActorId = dto.ActorId,
            NotificationType = dto.NotificationType,
            PostId = dto.PostId,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notification.Add(notification);

        await _context.SaveChangesAsync();

        var actorPhoto = actor.Photos
            .FirstOrDefault(p => p.IsMain);

        return new NotificationDto
        {
            Id = notification.Id,
            ActorId = notification.ActorId,
            ActorUserName = actor.UserName,
            ActorProfilePictureUrl = actorPhoto?.Url,
            Type = notification.NotificationType,
            PostId = notification.PostId,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt
        };
    }
}