using ImageUploadFeature.API.Models;

namespace ImageUploadFeature.API.Interfaces
{
    public interface IProfileService
    {
        Task<List<ImageDto>> GetImagesAsync(Guid profileId);
        Task<string?> UploadImagesAsync(Guid profileId, List<IFormFile> files);
        Task<bool> DeleteImageAsync(Guid profileId, Guid imageId);
    }
}
