using Microsoft.AspNetCore.Mvc;

using ImageUploadFeature.API.Interfaces;

namespace ImageUploadFeature.API.Controllers
{
    [ApiController]
    [Route("api/leads")]
    public class LeadsController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public LeadsController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("{id:guid}/images")]
        public async Task<IActionResult> GetImages(Guid id)
        {
            var images = await _profileService.GetImagesAsync(id);
            return Ok(images);
        }

        [HttpPost("{id:guid}/images")]
        public async Task<IActionResult> UploadImages(Guid id, [FromForm] List<IFormFile> files)
        {
            var error = await _profileService.UploadImagesAsync(id, files);
            if (error is not null)
                return BadRequest(new { Message = error });

            return Ok(new { Message = "Images uploaded successfully" });
        }

        [HttpDelete("{id:guid}/images/{imageId:guid}")]
        public async Task<IActionResult> DeleteImage(Guid id, Guid imageId)
        {
            var success = await _profileService.DeleteImageAsync(id, imageId);
            if (!success) 
                return NotFound(new { Message = "Image not found" });

            return NoContent();
        }
    }
}
