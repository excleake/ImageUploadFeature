using ImageUploadFeature.API.Data;
using ImageUploadFeature.API.Models;
using ImageUploadFeature.API.Entities;
using ImageUploadFeature.API.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace ImageUploadFeature.API.Services
{
    public class ProfileService : IProfileService
    {
        private readonly AppDbContext _db;

        private const int MaxImagesPerProfile = 10;
        private const int MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

        private static readonly HashSet<string> AllowedContentTypes =
        [
            "image/jpeg", "image/png", "image/gif", "image/webp"
        ];

        public ProfileService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<ImageDto>> GetImagesAsync(Guid profileId)
        {
            var profile = await _db.Profiles
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == profileId);

            if (profile is null) 
                return [];

            return profile.Images
                .Select(i => new ImageDto
                {
                    Id = i.Id,
                    Base64 = i.Base64,
                    FileName = i.FileName,
                    ContentType = i.ContentType
                })
                .ToList();
        }

        public async Task<string?> UploadImagesAsync(Guid profileId, List<IFormFile> files)
        {
            var profile = await _db.Profiles
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == profileId);

            if (profile is null) 
                return null;

            if (profile.Images.Count + files.Count > MaxImagesPerProfile)
            {
                return $"Cannot upload {files.Count} files. Max {MaxImagesPerProfile} images per profile allowed. " +
                       $"This profile already has {profile.Images.Count} images.";
            }

            foreach (var file in files)
            {
                if (!AllowedContentTypes.Contains(file.ContentType) || file.Length > MaxFileSizeBytes)
                    continue;

                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var base64 = Convert.ToBase64String(ms.ToArray());

                _db.Images.Add(new Image
                {
                    ProfileId = profile.Id,
                    Base64 = base64,
                    FileName = file.FileName,
                    ContentType = file.ContentType
                });
            }

            await _db.SaveChangesAsync();
            return null;
        }

        public async Task<bool> DeleteImageAsync(Guid profileId, Guid imageId)
        {
            var img = await _db.Images.FirstOrDefaultAsync(i => i.Id == imageId && i.ProfileId == profileId);
            if (img is null) 
                return false;

            _db.Images.Remove(img);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
