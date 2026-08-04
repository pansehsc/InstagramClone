using API.DTOs;
using API.DTOs.Photos;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class FollowsController(
    IFollowRepository followRepository,
    IUserRepository userRepository)
    : BaseApiController
{
    [HttpPost("{userId:guid}")]
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
            return Ok();

        return BadRequest("Problem following user.");
    }

    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult> UnfollowUser(Guid userId)
    {
        var currentUserId = User.GetUserId();

        var follow = await followRepository.GetFollowAsync(currentUserId, userId);

        if (follow == null)
            return NotFound();

        followRepository.Delete(follow);

        if (await followRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Problem unfollowing user.");
    }

    [HttpGet("{userId:guid}/followers")]
    public async Task<ActionResult<IEnumerable<FollowersDto>>> GetFollowers(Guid userId)
    {
        var followers = await followRepository.GetFollowersAsync(userId);

        var result = followers.Select(f => new FollowersDto
        {
            UserId = f.Follower.Id,
            UserName = f.Follower.UserName,
            ProfilePictureUrl = f.Follower.ProfilePictureUrl
        });

        return Ok(result);
    }

    [HttpGet("{userId:guid}/following")]
    public async Task<ActionResult<IEnumerable<FollowersDto>>> GetFollowing(Guid userId)
    {
        var following = await followRepository.GetFollowingAsync(userId);

        var result = following.Select(f => new FollowersDto
        {
            UserId = f.Following.Id,
            UserName = f.Following.UserName,
            ProfilePictureUrl = f.Following.ProfilePictureUrl
        });

        return Ok(result);
    }
}