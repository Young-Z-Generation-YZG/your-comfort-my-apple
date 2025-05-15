

using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace YGZ.Catalog.Application.Abstractions.Uploading;

public interface IUploadImageService
{
    Task<ImageUploadResult> UploadImageFileAsync(IFormFile file);
    Task<DeletionResult> DeleteImageFileAsync(string publicId);
    Task<ListResourcesResult> GetImages();
}
