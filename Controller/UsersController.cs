using API.DTOs.Users;
using API.Extensions;
using API.Interfaces;
using API.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using API.DTOs.Posts;
using API.DTOs.Account;
namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
{
    [HttpGet("me")]
    public async Task<ActionResult<UserProfileDto>> GetCurrentUser()
    {
        var userId = User.GetUserId();
        var user = await userRepository.GetUserWithPhotosAsync(userId);
        if (user == null)
            return NotFound();
        var userDto = mapper.Map<UserProfileDto>(user);
        userDto.Posts = user.Posts 
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => mapper.Map<PostDto>(p))
            .ToList();
            if(userDto.Posts.Count == 0 ){ userDto.Posts =[];}
        return Ok(userDto);
    }
    // [AllowAnonymous]
    // [HttpGet("{userName}")]
    // public async Task<ActionResult<UserProfileDto>> GetUser(string userName)
    // {
    //     var user = await userRepository.GetUserByUserNameWithPhotosAsync(userName);
    //     if (user == null)
    //         return NotFound();
    //     var userDto = mapper.Map<UserProfileDto>(user);
    //     return Ok(userDto);
    // }
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<UserProfileDto>>> SearchUsers(
    [FromQuery] string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return BadRequest("Search text is required.");
        var users = await userRepository.SearchUsersAsync(username);
        var result = mapper.Map<IEnumerable<UserProfileDto>>(users);
        return Ok(result);
    }
    [HttpPut]
    public async Task<ActionResult> UpdateUser(UpdateUserDto updateUserDto)
    {
        var userId = User.GetUserId();
        var user = await userRepository.GetCurrentUserAsync(userId);
        if (user == null)
            return NotFound();
        mapper.Map(updateUserDto, user);
        userRepository.Update(user);
        if (await userRepository.SaveAllAsync())
            return Ok(user);
        return BadRequest("Failed to update profile.");
    }
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserProfileDto>> GetUserProfile(Guid userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            return NotFound("User not found.");
        var userDto = mapper.Map<UserProfileDto>(user);
        userDto.Posts = user.Posts
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => mapper.Map<PostDto>(p))
            .ToList();
        return Ok(userDto);
    }
}
