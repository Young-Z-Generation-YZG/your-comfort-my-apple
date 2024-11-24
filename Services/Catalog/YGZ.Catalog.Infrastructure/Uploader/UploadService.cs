

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using YGZ.Catalog.Application.Core.Abstractions.Uploading;

namespace YGZ.Catalog.Infrastructure.Uploading;

public class UploadService : IUploadService
{

    private readonly Cloudinary _cloudinary;

    public UploadService(IOptions<CloudinarySettings> settings)
    {
        var account = new Account(
            settings.Value.CloudName,
            settings.Value.ApiKey,
            settings.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(account);
    }

    public async Task<ListResourcesResult> GetImages()
    {
        var images = _cloudinary.ListResourcesByAssetFolder("microservices-apple");

        await Task.CompletedTask;

        return images;
    }

    public async Task<ImageUploadResult> UploadImageFileAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();

        try
        {
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();

                var fileName = file.FileName.Split(".")[0];

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "microservices-apple",
                    DisplayName = fileName
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        } catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<DeletionResult> DeleteImageFileAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);

        var reuslt = await _cloudinary.DestroyAsync(deleteParams);

        return reuslt;
    }
}
