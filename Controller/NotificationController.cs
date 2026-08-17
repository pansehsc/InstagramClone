using API.DTOs.Notifications;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class NotificationsController(
    INotificationRepository notificationRepository,
    IMapper mapper)
    : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotifications()
    {
        var userId = User.GetUserId();

        var notifications =
            await notificationRepository
                .GetUserNotificationsAsync(userId);

        var result =
            mapper.Map<IEnumerable<NotificationDto>>(notifications);

        return Ok(result);
    }

    [HttpPatch("{id:guid}/read")]
    public async Task<ActionResult> MarkAsRead(Guid id)
    {
        var userId = User.GetUserId();

        var notification =
            await notificationRepository.GetByIdAsync(id);

        if (notification == null)
            return NotFound();

        // Don't allow users to modify someone else's notification
        if (notification.UserId != userId)
            return Forbid();

        notification.IsRead = true;

        if (await notificationRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Problem marking notification as read.");
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteNotification(Guid id)
    {
        var userId = User.GetUserId();

        var notification =
            await notificationRepository.GetByIdAsync(id);

        if (notification == null)
            return NotFound();

        if (notification.UserId != userId)
            return Forbid();

        notificationRepository.Delete(notification);

        if (await notificationRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Problem deleting notification.");
    }
}