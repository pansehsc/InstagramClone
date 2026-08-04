using API.DTOs.Comments;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class CommentsController(
    ICommentRepository commentRepository,
    IPostRepository postRepository,
    IUserRepository userRepository)
    : BaseApiController
{
    [HttpPost("/api/posts/{postId:guid}/comments")]
    public async Task<ActionResult<CommentDto>> AddComment(
    Guid postId,
    CreateCommentDto dto)
    {
        var userId = User.GetUserId();

        var user = await userRepository.GetByIdAsync(userId);

        if (user == null)
            return Unauthorized();

        var post = await postRepository.GetByIdAsync(postId);

        if (post == null)
            return NotFound("Post not found.");

        var comment = new Comment
        {
            Content = dto.Content,
            CreatedById = user.Id,
            PostId = post.Id
        };

        commentRepository.Add(comment);

        if (!await commentRepository.SaveAllAsync())
            return BadRequest("Problem adding comment.");

        return Ok(new CommentDto
        {
            Id = comment.Id,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            UserId = user.Id,
            UserName = user.UserName,
            ProfilePictureUrl = user.ProfilePictureUrl
        });
    }

    [HttpGet("/api/posts/{postId:guid}/comments")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(Guid postId)
    {
        var comments = await commentRepository.GetByPostIdAsync(postId);

        var result = comments.Select(c => new CommentDto
        {
            Id = c.Id,
            Content = c.Content,
            CreatedAt = c.CreatedAt,
            UserId = c.CreatedById,
            UserName = c.User.UserName,
            ProfilePictureUrl = c.User.ProfilePictureUrl
        });

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
            return NoContent();

        return BadRequest("Problem deleting comment.");
    }
}