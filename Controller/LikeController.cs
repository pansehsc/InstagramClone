using API.DTOs.Likes;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class LikesController(
    ILikeRepository likeRepository,
    IPostRepository postRepository)
    : BaseApiController
{
    [HttpPost("/api/posts/{postId:guid}/like")]
    public async Task<ActionResult> LikePost(Guid postId)
    {
        var userId = User.GetUserId();

        var post = await postRepository.GetByIdAsync(postId);

        if (post == null)
            return NotFound("Post not found.");

        var existingLike = await likeRepository.GetLikeAsync(userId, postId);

        if (existingLike != null)
            return BadRequest("You already liked this post.");

        var like = new Like
        {
            CreatedById = userId,
            PostId = postId
        };

        likeRepository.Add(like);

        if (await likeRepository.SaveAllAsync())
            return Ok();

        return BadRequest("Problem liking post.");
    }

    [HttpDelete("/api/posts/{postId:guid}/like")]
    public async Task<ActionResult> UnlikePost(Guid postId)
    {
        var userId = User.GetUserId();

        var like = await likeRepository.GetLikeAsync(userId, postId);

        if (like == null)
            return NotFound();

        likeRepository.Delete(like);

        if (await likeRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Problem removing like.");
    }

    [HttpGet("/api/posts/{postId:guid}/likes/count")]
    public async Task<ActionResult<int>> GetLikesCount(Guid postId)
    {
        var count = await likeRepository.GetLikesCountAsync(postId);

        return Ok(count);
    }

    [HttpGet("/api/posts/{postId:guid}/likes")]
    public async Task<ActionResult<IEnumerable<PostLikeDto>>> GetLikes(Guid postId)
    {
        var likes = await likeRepository.GetPostLikesAsync(postId);

        var result = likes.Select(x => new PostLikeDto
        {
            UserId = x.CreatedById,
            UserName = x.User.UserName,
            ProfilePictureUrl = x.User.ProfilePictureUrl
        });

        return Ok(result);
    }
}