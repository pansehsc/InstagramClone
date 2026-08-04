using API.DTOs.Posts;
using API.DTOs.Photos;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class PostsController(
    IPostRepository postRepository,
    IUserRepository userRepository)
    : BaseApiController
{
    // Create Post
    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePost(CreatePostDto dto)
    {
        var userId = User.GetUserId();

        var user = await userRepository.GetByIdAsync(userId);

        if (user == null)
            return NotFound();

        var post = new Post
        {
            Caption = dto.Caption,
            Hashtags = dto.Hashtags,
            UserId = user.Id
        };

        postRepository.Add(post);

        if (!await postRepository.SaveAllAsync())
            return BadRequest("Problem creating post");

        return Ok(new PostDto
        {
            Id = post.Id,
            Caption = post.Caption,
            Hashtags = post.Hashtags,
            CreatedAt = post.CreatedAt,
            UserName = user.UserName,
            ProfilePictureUrl = user.ProfilePictureUrl,
            LikesCount = 0,
            CommentsCount = 0,
            Photos = []
        });
    }

    // Get Feed
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts()
    {
        var posts = await postRepository.GetAllAsync();

        var result = posts.Select(post => new PostDto
        {
            Id = post.Id,
            Caption = post.Caption,
            Hashtags = post.Hashtags,
            CreatedAt = post.CreatedAt,
            UserName = post.User.UserName,
            ProfilePictureUrl = post.User.ProfilePictureUrl,
            LikesCount = post.Likes.Count,
            CommentsCount = post.Comments.Count,
            Photos = post.Photos.Select(photo => new PhotoDto
            {
                Id = photo.Id,
                Url = photo.Url,
                IsMain = photo.IsMain
            }).ToList()
        });

        return Ok(result);
    }

    // Get Single Post
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PostDto>> GetPost(Guid id)
    {
        var post = await postRepository.GetByIdAsync(id);

        if (post == null)
            return NotFound();

        return Ok(new PostDto
        {
            Id = post.Id,
            Caption = post.Caption,
            Hashtags = post.Hashtags,
            CreatedAt = post.CreatedAt,
            UserName = post.User.UserName,
            ProfilePictureUrl = post.User.ProfilePictureUrl,
            LikesCount = post.Likes.Count,
            CommentsCount = post.Comments.Count,
            Photos = post.Photos.Select(photo => new PhotoDto
            {
                Id = photo.Id,
                Url = photo.Url,
                IsMain = photo.IsMain
            }).ToList()
        });
    }

    // Update Post
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdatePost(Guid id, CreatePostDto dto)
    {
        var userId = User.GetUserId();

        var post = await postRepository.GetByIdAsync(id);

        if (post == null)
            return NotFound();

        if (post.UserId != userId)
            return Forbid();

        post.Caption = dto.Caption;
        post.Hashtags = dto.Hashtags;

        postRepository.Update(post);

        if (await postRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Problem updating post");
    }

    // Delete Post
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeletePost(Guid id)
    {
        var userId = User.GetUserId();

        var post = await postRepository.GetByIdAsync(id);

        if (post == null)
            return NotFound();

        if (post.UserId != userId)
            return Forbid();

        postRepository.Delete(post);

        if (await postRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Problem deleting post");
    }
}