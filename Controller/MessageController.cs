using API.DTOs.Messages;
using API.DTOs.Notifications;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class MessagesController(
    IMessageRepository messageRepository,
    IUserRepository userRepository,
    INotificationService notificationService,
    IMapper mapper)
    : BaseApiController
{
    // Send message
    [HttpPost]
    public async Task<ActionResult<MessageDto>> SendMessage(
        SendMessageDto dto)
    {
        var senderId = User.GetUserId();
        if (senderId == dto.ReceiverId)
            return BadRequest("You cannot send a message to yourself.");
        var receiver = await userRepository.GetByIdAsync(dto.ReceiverId);
        if (receiver == null)
            return NotFound("Receiver not found.");
        var message = mapper.Map<Messaging>(dto);
        message.SenderId = senderId;
        message.ReceiverId = dto.ReceiverId;
        messageRepository.Add(message);
        if (!await messageRepository.SaveAllAsync())
            return BadRequest("Problem sending message.");
        NotificationDto? notificationDto = null;
        notificationDto =
            await notificationService.CreateNotificationAsync(
                new CreateNotificationDto
                {
                    UserId = dto.ReceiverId,
                    ActorId = senderId,
                    NotificationType = "Message"
                });
        var result = mapper.Map<MessageDto>(message);
        return Ok(new
        {
            Message = result,
            Notification = notificationDto
        });
    }

    // Get conversation
    [HttpGet("conversation/{userId:guid}")]
    public async Task<ActionResult<IEnumerable<MessageDto>>>
        GetConversation(Guid userId)
    {
        var currentUserId = User.GetUserId();

        if (currentUserId == userId)
            return BadRequest("You cannot have a conversation with yourself.");

        var user = await userRepository.GetByIdAsync(userId);

        if (user == null)
            return NotFound("User not found.");

        var messages =
            await messageRepository.GetConversationAsync(
                currentUserId,
                userId);

        var result =
            mapper.Map<IEnumerable<MessageDto>>(messages);

        return Ok(result);
    }

    // // Get inbox
    // [HttpGet("inbox")]
    // public async Task<ActionResult<IEnumerable<MessageDto>>> GetInbox()
    // {
    //     var currentUserId = User.GetUserId();
    //     var messages =
    //         await messageRepository.GetInboxAsync(currentUserId);
    //     var result =
    //         mapper.Map<IEnumerable<MessageDto>>(messages);
    //     return Ok(result);
    // }

    // Mark message as read
    [HttpPut("{id:guid}/read")]
    public async Task<ActionResult> MarkAsRead(Guid id)
    {
        var currentUserId = User.GetUserId();
        var message =
            await messageRepository.GetByIdAsync(id);
        if (message == null)
            return NotFound();
        if (message.ReceiverId != currentUserId)
            return Forbid();
        if (message.IsRead)
            return Ok("already marked");
        message.IsRead = true;
        message.ReadAt = DateTime.UtcNow;
        if (!await messageRepository.SaveAllAsync())
            return BadRequest("Problem marking message as read.");
        return Ok("mark as read");
    }

    // Delete message for current user
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteMessage(Guid id)
    {
        var currentUserId = User.GetUserId();
        var message =
            await messageRepository.GetByIdAsync(id);
        if (message == null)
            return NotFound();
        if (message.SenderId == currentUserId)
        {
            message.SenderDeleted = true;
        }
        else if (message.ReceiverId == currentUserId)
        {
            message.RecipientDeleted = true;
        }
        else
        {
            return Forbid();
        }
        if (!await messageRepository.SaveAllAsync())
            return BadRequest("Problem deleting message.");
        return Ok("The message has been deleted.");
    }
}

