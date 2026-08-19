using API.DTOs.Likes;
using API.DTOs.Notifications;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Mapping;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class LikesController(
    ILikeRepository likeRepository,
    IPostRepository postRepository,
    INotificationService notificationService,
    IMapper mapper)
    : BaseApiController
{
    // add like
    [HttpPost("/api/posts/{postId:guid}/like")]
    public async Task<ActionResult> LikePost(Guid postId)
    {
        var userId = User.GetUserId();

        var post = await postRepository.GetByIdAsync(postId);

        if (post == null)
            return NotFound("Post not found.");

        var existingLike = await likeRepository.GetLikeAsync(userId, postId);

        if (existingLike != null)
            return Ok("You already liked this post.");

        var like = new Like
        {
            CreatedById = userId,
            PostId = postId
        };
        likeRepository.Add(like);
        if (!await likeRepository.SaveAllAsync())
            return BadRequest("Problem liking post.");
        NotificationDto? notificationDto = null;
        if (post.UserId != userId)
        {
            notificationDto =
                await notificationService.CreateNotificationAsync(
                    new CreateNotificationDto
                    {
                        UserId = post.UserId,
                        ActorId = userId,
                        NotificationType = "Like",
                        PostId = postId
                    });
        }
        return Ok(new
        {
            Message = "Post liked successfully.",
            Notification = notificationDto
        });
    }
    //remove like
    [HttpDelete("/api/posts/{postId:guid}/like")]
    public async Task<ActionResult> UnlikePost(Guid postId)
    {
        var userId = User.GetUserId();

        var like = await likeRepository.GetLikeAsync(userId, postId);
        if (like == null)
            return NotFound();
        if (like.CreatedById != userId)
            return Forbid();

        likeRepository.Delete(like);

        if (await likeRepository.SaveAllAsync())
            return Ok("unlike post successfully");

        return BadRequest("Problem removing like.");
    }

    [HttpGet("/api/posts/{postId:guid}/likes/count")]
    public async Task<ActionResult<int>> GetLikesCount(Guid postId)
    {
        var count = await likeRepository.GetLikesCountAsync(postId);

        return Ok($"no of users who like this post:{count}");
    }

    [HttpGet("/api/posts/{postId:guid}/likes")]
    public async Task<ActionResult<IEnumerable<PostLikeDto>>> GetLikes(Guid postId)
    {
        var likes = await likeRepository.GetPostLikesAsync(postId);
        var result = mapper.Map<IEnumerable<PostLikeDto>>(likes);
        return Ok(result);
    }
}