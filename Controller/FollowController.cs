using API.DTOs;
using API.DTOs.Follows;
using API.DTOs.Photos;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Mapping;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class FollowsController(
    IFollowRepository followRepository,
    IUserRepository userRepository,
    IMapper mapper)
    : BaseApiController
{

    [HttpPost("{userId:guid}")] //user who will follow
    public async Task<ActionResult> FollowUser(Guid userId)
    {
        var currentUserId = User.GetUserId();

        if (currentUserId == userId)
            return BadRequest("You cannot follow yourself.");

        var user = await userRepository.GetByIdAsync(userId);

        if (user == null)
            return NotFound("User not found.");

        var follow = await followRepository.GetFollowAsync(currentUserId, userId);

        if (follow != null)
            return BadRequest("You already follow this user.");

        follow = new Follow
        {
            FollowerId = currentUserId,
            FollowingId = userId
        };

        followRepository.Add(follow);

        if (await followRepository.SaveAllAsync())
            return Ok("add follow successfully");

        return BadRequest("Problem following user.");
    }

    [HttpDelete("{userId:guid}")] //user who will follow
    public async Task<ActionResult> UnfollowUser(Guid userId)
    {
        var currentUserId = User.GetUserId();

        var follow = await followRepository.GetFollowAsync(currentUserId, userId);

        if (follow == null)
            return NotFound();

        followRepository.Delete(follow);

        if (await followRepository.SaveAllAsync())
            return Ok("unfollow successfully");

        return BadRequest("Problem unfollowing user.");
    }

    [HttpGet("{userId:guid}/followers")]
    public async Task<ActionResult<IEnumerable<FollowersDto>>> GetFollowers(Guid userId)
    {
        var followers = await followRepository.GetFollowersAsync(userId);
        var result = mapper.Map<IEnumerable<FollowersDto>>(followers);
        return Ok(result);
    }

    [HttpGet("{userId:guid}/following")]
    public async Task<ActionResult<IEnumerable<FollowersDto>>> GetFollowing(Guid userId)
    {
        var following = await followRepository.GetFollowingAsync(userId);
        var result = mapper.Map<IEnumerable<FollowingDto>>(following);
        return Ok(result);
    }
    [HttpGet("status/{userId:guid}")]
    public async Task<ActionResult<FollowStateDto>> GetFollowStatus(Guid userId)
    {
        var currentUserId = User.GetUserId();
        if (currentUserId == userId)
            return BadRequest("You cannot check follow status for yourself.");
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            return NotFound("User not found.");
        var isFollowing = await followRepository.IsFollowingAsync(
            currentUserId,
            userId);
        return Ok(new FollowStateDto
        {
            IsFollowing = isFollowing
        });
    }
}