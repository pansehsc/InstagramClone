using API.DTOs.Posts;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace API.Controllers;

[Authorize]
public class PostsController(
    IPostRepository postRepository,
    IUserRepository userRepository,
    IPhotoService photoService,
    IMapper mapper)
    : BaseApiController
{
    // Create Post
    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePost(
        [FromForm] CreatePostDto dto)
    {
        var userId = User.GetUserId();

        var user = await userRepository.GetByIdAsync(userId);

        if (user == null)
            return NotFound("User not found.");

        if (dto.Photos == null || dto.Photos.Count == 0)
            return BadRequest("A post must contain at least one photo.");

        var post = mapper.Map<Post>(dto);
        post.UserId = userId;

        foreach (var file in dto.Photos)
        {
            var uploadResult = await photoService.AddPhotoAsync(file);

            if (uploadResult.Error != null)
                return BadRequest(uploadResult.Error.Message);

            if (uploadResult.SecureUrl == null)
                return BadRequest("Photo upload failed.");

            var photo = new Photo
            {
                Url = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId,

                // First photo is the post's main/cover photo
                IsMain = post.Photos.Count == 0
            };

            post.Photos.Add(photo);
        }

        postRepository.Add(post);

        if (!await postRepository.SaveAllAsync())
            return BadRequest("Problem creating post.");

        var result = mapper.Map<PostDto>(post);

        return Ok(result);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts()
    {
        var posts = await postRepository.GetAllAsync();
        var result = mapper.Map<IEnumerable<PostDto>>(posts);
        return Ok(result);

    }

    // Get Single Post
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PostDto>> GetPost(Guid id)
    {
        var post = await postRepository.GetByIdAsync(id);
        if (post == null)
            return NotFound();
        var postDto = mapper.Map<PostDto>(post);
        return Ok(postDto);
    }

    // Update Post
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdatePost(Guid id, UpdatePostDto dto)
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
            return Ok("Post updated successfully");

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
            return Ok("Post deleted successfully");
        
        return BadRequest("Problem deleting post");
    }
    // get posts shared by users you follow
    [HttpGet("feed")]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetFeed()
    {
        var currentUserId = User.GetUserId();
        var posts = await postRepository.GetFeedAsync(currentUserId);
        var result = mapper.Map<IEnumerable<PostDto>>(posts);
        return Ok(result);
    }
}