using System.Security.Cryptography;
using API.Controllers;
using API.DTOs;
using API.DTOs.Account;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller
{
    public class AccountController(
    IUserRepository userRepository,
    ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await userRepository.GetByEmailAsync(registerDto.Email) != null)
            {
                return BadRequest("Email is already taken");
            }

            if (await userRepository.GetByUserNameAsync(registerDto.UserName) != null)
            {
                return BadRequest("Username is already taken");
            }

            using var hmac = new HMACSHA512();

            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Gender = registerDto.Gender,
                DateOfBirth = registerDto.DateOfBirth,
                Country = registerDto.Country,
                City = registerDto.City,

                PasswordHash = hmac.ComputeHash(
                    System.Text.Encoding.UTF8.GetBytes(registerDto.Password)),

                PasswordSalt = hmac.Key,

                LastActive = DateTime.UtcNow
            };

            userRepository.Add(user);

            if (!await userRepository.SaveAllAsync())
            {
                return BadRequest("Problem creating user");
            }

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Token = tokenService.CreateToken(user),
                ProfilePictureUrl = user.ProfilePictureUrl
            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null)
                return Unauthorized("Invalid email");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(
                System.Text.Encoding.UTF8.GetBytes(loginDto.Password));

            if (!computedHash.SequenceEqual(user.PasswordHash))
                return Unauthorized("Invalid password");

            user.LastActive = DateTime.UtcNow;

            userRepository.Update(user);

            await userRepository.SaveAllAsync();

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Token = tokenService.CreateToken(user),
                ProfilePictureUrl = user.ProfilePictureUrl
            };
        }
    }
}
