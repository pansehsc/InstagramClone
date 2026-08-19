using API.DTOs.Comments;
using API.DTOs.Notifications;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class CommentsController(
    ICommentRepository commentRepository,
    IPostRepository postRepository,
    IUserRepository userRepository,
    INotificationService notificationService,
    IMapper mapper)
    : BaseApiController
{
    [HttpPost("/api/posts/{postId:guid}/comments")]
    public async Task<ActionResult<CommentDto>> AddComment(
    Guid postId,
    CreateCommentDto createCommentDto)
    {
        var userId = User.GetUserId();
        var user = await userRepository.GetByIdAsync(userId);

        if (user == null)
            return Unauthorized();

        var post = await postRepository.GetByIdAsync(postId);

        if (post == null)
            return NotFound("Post not found.");

        var comment = mapper.Map<Comment>(createCommentDto);

        comment.CreatedById = userId;
        comment.PostId = postId;

        commentRepository.Add(comment);

        if (!await commentRepository.SaveAllAsync())
            return BadRequest("Problem adding comment.");
        NotificationDto? notificationDto = null;
        if (post.UserId != userId)
        {
            notificationDto = await notificationService.CreateNotificationAsync(
                new CreateNotificationDto
                {
                    UserId = post.UserId,
                    ActorId = userId,
                    NotificationType = "Comment",
                    PostId = postId
                });
        }
        var result = mapper.Map<CommentDto>(comment);
        var profilePhoto = user.Photos.FirstOrDefault(p => p.IsMain);
        result.ProfilePictureUrl = profilePhoto?.Url;
        return Ok(new
        {
            Comment = result,
            Notification = notificationDto
        });
    }

    [HttpGet("/api/posts/{postId:guid}/comments")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(Guid postId)
    {
        var comments = await commentRepository.GetByPostIdAsync(postId);
        var result = mapper.Map<IEnumerable<CommentDto>>(comments);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteComment(Guid id)
    {
        var userId = User.GetUserId();
        var comment = await commentRepository.GetByIdAsync(id);
        if (comment == null)
            return NotFound();
        if (comment.CreatedById != userId)
            return Forbid();
        commentRepository.Delete(comment);
        if (await commentRepository.SaveAllAsync())
            return Ok("delete comment successfully.");
        return BadRequest("Problem deleting comment.");
    }
}