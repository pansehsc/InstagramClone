using API.DTOs.Photos;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class PhotosController(
    IPhotoService photoService,
    IUserRepository userRepository)
    : BaseApiController
{
    // Upload a new photo
    [HttpPost]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var userId = User.GetUserId();

        var user = await userRepository.GetUserWithPhotosAsync(userId);

        if (user == null)
            return NotFound("User not found.");

        var uploadResult = await photoService.AddPhotoAsync(file);

        if (uploadResult.Error != null)
            return BadRequest(uploadResult.Error.Message);

        if (uploadResult.SecureUrl == null)
            return BadRequest("Photo upload failed.");

        var photo = new Photo
        {
            Url = uploadResult.SecureUrl.AbsoluteUri,
            PublicId = uploadResult.PublicId,
            UserId = user.Id
        };

        // First uploaded photo becomes the profile photo
        if (!user.Photos.Any())
            photo.IsMain = true;

        user.Photos.Add(photo);

        if (await userRepository.SaveAllAsync())
        {
            return Ok(new PhotoDto
            {
                Id = photo.Id,
                Url = photo.Url,
                IsMain = photo.IsMain
            });
        }

        return BadRequest("Problem adding photo.");
    }

    // Set profile photo
    [HttpPut("{photoId}/set-main")]
    public async Task<ActionResult> SetMainPhoto(Guid photoId)
    {
        var userId = User.GetUserId();

        var user = await userRepository.GetUserWithPhotosAsync(userId);

        if (user == null)
            return NotFound();

        var photo = user.Photos.SingleOrDefault(x => x.Id == photoId);

        if (photo == null)
            return NotFound("Photo not found.");

        if (photo.IsMain)
            return BadRequest("This photo is already the main photo.");

        var currentMain = user.Photos.SingleOrDefault(x => x.IsMain);

        if (currentMain != null)
            currentMain.IsMain = false;

        photo.IsMain = true;

        if (await userRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to set main photo.");
    }

    // Delete photo
    [HttpDelete("{photoId}")]
    public async Task<ActionResult> DeletePhoto(Guid photoId)
    {
        var userId = User.GetUserId();

        var user = await userRepository.GetUserWithPhotosAsync(userId);

        if (user == null)
            return NotFound();

        var photo = user.Photos.SingleOrDefault(x => x.Id == photoId);

        if (photo == null)
            return NotFound("Photo not found.");

        if (photo.IsMain)
            return BadRequest("You cannot delete your main profile photo.");

        if (!string.IsNullOrEmpty(photo.PublicId))
        {
            var result = await photoService.DeletePhotoAsync(photo.PublicId);

            if (result.Error != null)
                return BadRequest(result.Error.Message);
        }

        user.Photos.Remove(photo);

        if (await userRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to delete photo.");
    }
}