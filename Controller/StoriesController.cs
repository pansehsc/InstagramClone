using API.DTOs.Stories;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Repositories;
using API.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[Authorize]
public class StoriesController(
    IStoryRepository storyRepository,
    IPhotoService photoService,
    IMapper mapper)
    : BaseApiController
{

    // CREATE STORY
    [HttpPost]
    public async Task<ActionResult<StoryDto>> CreateStory([FromForm] CreateStoryDto dto)
    {
        var userId = User.GetUserId();

        // Story must contain text or an image
        if (string.IsNullOrWhiteSpace(dto.Content)
            && dto.File == null)
        {
            return BadRequest(
                "Story must contain text or an image.");
        }

        var story = new Story
        {
            UserId = userId,
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddHours(24)
        };


        if (dto.File != null)
        {
            var result =
                await photoService.AddPhotoAsync(dto.File);

            if (result.Error != null)
                return BadRequest(result.Error.Message);

            story.Url =
                result.SecureUrl.AbsoluteUri;

            story.PublicId =
                result.PublicId;
        }

        storyRepository.Add(story);

        if (!await storyRepository.SaveAllAsync())
            return BadRequest("Problem creating story.");


        var createdStory =
            await storyRepository.GetByIdAsync(story.Id);

        if (createdStory == null)
            return BadRequest("Problem creating story.");

        var resultDto =
            mapper.Map<StoryDto>(createdStory);

        return Ok(resultDto);
    }

    // GET ALL ACTIVE STORIES
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StoryDto>>> GetStories()
    {
        var stories =
            await storyRepository.GetActiveStoriesAsync();

        var result =
            mapper.Map<IEnumerable<StoryDto>>(stories);

        return Ok(result);
    }

    // GET MY STORIES
    [HttpGet("mine")]
    public async Task<ActionResult<IEnumerable<StoryDto>>> GetMyStories()
    {
        var userId = User.GetUserId();

        var stories =
            await storyRepository.GetUserStoriesAsync(userId);

        var result =
            mapper.Map<IEnumerable<StoryDto>>(stories);

        return Ok(result);
    }

    // GET ONE STORY
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StoryDto>> GetStory(Guid id)
    {
        var story =
            await storyRepository.GetByIdAsync(id);

        if (story == null)
            return NotFound("Story not found.");

        if (story.ExpiresAt <= DateTime.UtcNow)
            return NotFound("Story has expired.");

        var result =
            mapper.Map<StoryDto>(story);

        return Ok(result);
    }

    // DELETE STORY
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteStory(Guid id)
    {
        var userId = User.GetUserId();

        var story =
            await storyRepository.GetByIdAsync(id);

        if (story == null)
            return NotFound("Story not found.");

        if (story.UserId != userId)
            return Forbid();

        if (!string.IsNullOrEmpty(story.PublicId))
        {
            await photoService.DeletePhotoAsync(
                story.PublicId);
        }

        storyRepository.Delete(story);

        if (!await storyRepository.SaveAllAsync())
            return BadRequest("Problem deleting story.");

        return NoContent();
    }
    //GET ALL STORY OF USERS YOU FOLLOW
    [HttpGet("following")]
    public async Task<ActionResult<IEnumerable<StoryDto>>> GetStoriesFromFollowing()
    {
        var userId = User.GetUserId();

        var stories =
            await storyRepository.GetStoriesFromFollowingAsync(userId);

        var result = mapper.Map<IEnumerable<StoryDto>>(stories);

        return Ok(result);
    }
}