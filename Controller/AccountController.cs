using System.Security.Cryptography;
using API.Controllers;
using API.DTOs;
using API.DTOs.Account;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using API.Mapping;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.DTOs.Photos;
using API.Extensions;

namespace API.Controller
{
    public class AccountController(
    IUserRepository userRepository,
    ITokenService tokenService,
    IEmailService emailService,
    IPhotoService photoService,
    IPhotoRepository photoRepository,
    IMapper mapper) : BaseApiController
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
            var user = mapper.Map<User>(registerDto);
            user.PasswordHash = hmac.ComputeHash(
                System.Text.Encoding.UTF8.GetBytes(registerDto.Password)
            );
            user.PasswordSalt = hmac.Key;
            user.LastActive = DateTime.UtcNow;
            userRepository.Add(user);
            var saveUser = await userRepository.SaveAllAsync();
            if (!saveUser)
            {
                return BadRequest("Problem creating user");
            }
            var userDto = mapper.Map<UserDto>(user); // user ==> dto
            userDto.Token = tokenService.CreateToken(user);
            return Ok(userDto);
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
            var userDto = mapper.Map<UserDto>(user);
            userDto.Token = tokenService.CreateToken(user);
            return userDto;
        }
        // Change Password apis
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgetPassword(ForgetPasswordDto dto)
        {
            //Note: The appsettings application requires a valid email address and password to send the token via email.
            var user = await userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                return Ok("If the email exists, a reset token has been sent.");
            var token = Convert.ToBase64String(
                RandomNumberGenerator.GetBytes(32));
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpires =
                DateTime.UtcNow.AddMinutes(15);
            userRepository.Update(user);
            if (!await userRepository.SaveAllAsync())
                return BadRequest("Problem creating reset token.");
            await emailService.SendPasswordResetEmailAsync(
                user.Email,
                token);
            return Ok("If the email exists, a mail with token has been sent.");
        }
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(
        ResetPasswordDto dto)
        {
            var user = await userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                return BadRequest("Invalid reset request.");
            if (user.PasswordResetToken == null ||
                user.PasswordResetTokenExpires == null)
            {
                return BadRequest("Invalid reset request.");
            }
            if (user.PasswordResetToken != dto.Token)
                return BadRequest("Invalid reset token.");
            if (user.PasswordResetTokenExpires < DateTime.UtcNow)
                return BadRequest("Reset token has expired.");
            using var hmac = new HMACSHA512();
            user.PasswordHash = hmac.ComputeHash(
                System.Text.Encoding.UTF8.GetBytes(dto.NewPassword));
            user.PasswordSalt = hmac.Key;
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpires = null;
            userRepository.Update(user);
            if (!await userRepository.SaveAllAsync())
                return BadRequest("Problem resetting password.");
            return Ok(new
            {
                Message = "Password has been reset successfully.",
                UserName = user.UserName,
                Email = user.Email
            });
        }

        [HttpPost("profile-photo")]
        public async Task<ActionResult<PhotoDto>> AddProfilePhoto(
        IFormFile file)
        {
            var userId = User.GetUserId();

            var user = await userRepository.GetUserWithPhotosAsync(userId);

            if (user == null)
                return NotFound("User not found.");

            if (file == null || file.Length == 0)
                return BadRequest("Please provide a photo.");


            // Upload image to Cloudinary
            var uploadResult = await photoService.AddPhotoAsync(file);

            if (uploadResult.Error != null)
                return BadRequest(uploadResult.Error.Message);

            if (uploadResult.SecureUrl == null)
                return BadRequest("Photo upload failed.");

            var currentMain = user.Photos
                .SingleOrDefault(p => p.IsMain);

            if (currentMain != null)
            {
                currentMain.IsMain = false;
            }

            var photo = new Photo
            {
                Url = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId,
                UserId = userId,
                IsMain = true
            };


            photoRepository.Add(photo);
            if (!await photoRepository.SaveAllAsync())
                return BadRequest("Problem adding profile photo.");


            var result = mapper.Map<PhotoDto>(photo);

            return Ok(result);
        }
        //     [HttpPut("profile-photo/{photoId}/main")]
        //     public async Task<ActionResult> SetMainPhoto(Guid photoId)
        //     {
        //         var userId = User.GetUserId();

        //         var user =
        //             await userRepository.GetUserWithPhotosAsync(userId);

        //         if (user == null)
        //             return NotFound("User not found.");

        //         var photo = user.Photos
        //             .SingleOrDefault(p => p.Id == photoId);

        //         if (photo == null)
        //             return NotFound("Photo not found.");

        //         if (photo.IsMain)
        //             return BadRequest(
        //                 "This photo is already the main photo.");

        //         var currentMain = user.Photos
        //             .SingleOrDefault(p => p.IsMain);

        //         if (currentMain != null)
        //             currentMain.IsMain = false;

        //         photo.IsMain = true;

        //         if (!await userRepository.SaveAllAsync())
        //             return BadRequest(
        //                 "Problem setting main photo.");

        //         return NoContent();
        //     }

        //     [HttpDelete("profile-photo/{photoId}")]
        //     public async Task<ActionResult> DeleteProfilePhoto(
        //         Guid photoId)
        //     {
        //         var userId = User.GetUserId();

        //         var user =
        //             await userRepository.GetUserWithPhotosAsync(userId);

        //         if (user == null)
        //             return NotFound("User not found.");

        //         var photo = user.Photos
        //             .SingleOrDefault(p => p.Id == photoId);

        //         if (photo == null)
        //             return NotFound("Photo not found.");

        //         if (photo.IsMain)
        //             return BadRequest(
        //                 "You cannot delete your main profile photo.");

        //         if (!string.IsNullOrEmpty(photo.PublicId))
        //         {
        //             var deleteResult =
        //                 await photoService.DeletePhotoAsync(
        //                     photo.PublicId);

        //             if (deleteResult.Error != null)
        //                 return BadRequest(
        //                     deleteResult.Error.Message);
        //         }

        //         photoRepository.Remove(photo);

        //         if (!await userRepository.SaveAllAsync())
        //             return BadRequest(
        //                 "Problem deleting profile photo.");

        //         return NoContent();
        //     }
    }
}
