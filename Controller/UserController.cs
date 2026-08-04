using API.DTOs.Users;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository) : BaseApiController
{
    [HttpGet("me")]
    public async Task<ActionResult<UserProfileDto>> GetCurrentUser()
    {
        var userId = User.GetUserId();

        var user = await userRepository.GetUserWithPhotosAsync(userId);

        if (user == null)
            return NotFound();

        var userDto = new UserProfileDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Bio = user.Bio,
            ProfilePictureUrl = user.ProfilePictureUrl,
            Followers = user.Followers.Count,
            Following = user.Following.Count,
            Posts = user.Posts.Count
        };

        return Ok(userDto);
    }

    [AllowAnonymous]
    [HttpGet("{userName}")]
    public async Task<ActionResult<UserProfileDto>> GetUser(string userName)
    {
        var user = await userRepository.GetUserByUserNameWithPhotosAsync(userName);

        if (user == null)
            return NotFound();

        var userDto = new UserProfileDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Bio = user.Bio,
            ProfilePictureUrl = user.ProfilePictureUrl,
            Followers = user.Followers.Count,
            Following = user.Following.Count,
            Posts = user.Posts.Count
        };

        return Ok(userDto);
    }
    [HttpPut]
    public async Task<ActionResult> UpdateUser(UpdateUserDto updateUserDto)
    {
        var userId = User.GetUserId();

        var user = await userRepository.GetCurrentUserAsync(userId);

        if (user == null)
            return NotFound();

        user.Bio = updateUserDto.Bio;
        user.Country = updateUserDto.Country;
        user.City = updateUserDto.City;
        user.ProfilePictureUrl = updateUserDto.ProfilePictureUrl;

        userRepository.Update(user);

        if (await userRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to update profile.");
    }
}