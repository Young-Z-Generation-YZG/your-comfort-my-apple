
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace YGZ.Catalog.Application.Core.Abstractions.Uploading;

public interface IUploadService
{
    Task<ImageUploadResult> UploadImageFileAsync(IFormFile file);
    Task<DeletionResult> DeleteImageFileAsync(string publicId);
    Task<ListResourcesResult> GetImages();

}
