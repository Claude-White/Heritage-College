using HFA.Models;
using Microsoft.AspNetCore.Authorization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Text.Json;

namespace HFA.Services
{
    public interface IImageService
    {
        Task<Models.Image?> CreateImageAsync(IFormFile file, string imageEditDataJson);
        Task<Models.Image?> GetImageAsync(int imageId);
    }

    public class ImageEdit
    {
        public float x { get; set; }
        public float y { get; set; }
        public float width { get; set; }
        public float height { get; set; }
        public float rotate { get; set; }
        public float scaleX { get; set; }
        public float scaleY { get; set; }
    }

    public class ImageService(IDbAccess dbAccess) : IImageService
    {
        private readonly IDbAccess _dbAccess = dbAccess;

        [Authorize(Policy = "HasEditPermissions")]
        public async Task<Models.Image?> CreateImageAsync(IFormFile file, string? imageEditDataJson)
        {
            // Check file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                throw new Exception("File must be an image!");
            }

            // Check file size
            var sizeLimitInMB = 5 * 1024 * 1024;
            if (file.Length > sizeLimitInMB)
            {
                throw new Exception("Image size must not exceed 5MB!");
            }

            // Prepare Image
            SixLabors.ImageSharp.Image imageSharpImage = await SixLabors.ImageSharp.Image.LoadAsync(file.OpenReadStream());

            // Crop Image
            if (imageEditDataJson != null)
            {
                // Parse the image Edit Data
                var imageEditData = JsonSerializer.Deserialize<ImageEdit>(imageEditDataJson);

                if (imageEditData != null)
                {
                    // Rotate Image
                    if (imageEditData.rotate != 0)
                    {
                        imageSharpImage.Mutate(x => x.Rotate(imageEditData.rotate));
                    }

                    // Crop Dimensions
                    int maxWidth = imageSharpImage.Width;
                    int maxHeight = imageSharpImage.Height;
                    int cropX = Math.Max(0, Math.Min((int)imageEditData.x, maxWidth));
                    int cropY = Math.Max(0, Math.Min((int)imageEditData.y, maxHeight));
                    int cropWidth = Math.Max(0, Math.Min((int)imageEditData.width, maxWidth - cropX));
                    int cropHeight = Math.Max(0, Math.Min((int)imageEditData.height, maxHeight - cropY));

                    // Crop Image
                    var cropBox = new Rectangle(cropX, cropY, cropWidth, cropHeight);
                    imageSharpImage.Mutate(x => x.Crop(cropBox));

                } else
                {
                    throw new ArgumentException("Invalid crop data", nameof(imageEditDataJson));
                }
            }

            return await _dbAccess.CreateImageAsync(imageSharpImage);
        }

        [AllowAnonymous]
        public async Task<Models.Image?> GetImageAsync(int imageId)
        {
            return await _dbAccess.GetImageAsync(imageId);
        }
    }
}