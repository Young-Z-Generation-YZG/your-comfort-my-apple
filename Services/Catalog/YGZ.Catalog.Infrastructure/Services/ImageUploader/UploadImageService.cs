
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using YGZ.Catalog.Application.Abstractions.Uploading;
using YGZ.Catalog.Infrastructure.Settings;

namespace YGZ.Catalog.Infrastructure.Services.ImageUploader;
public class UploadImageService : IUploadImageService
{
    private readonly Cloudinary _cloudinary;

    public UploadImageService(IOptions<CloudinarySettings> settings)
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
        var searchParams = new Search(_cloudinary.Api)
            .Expression("folder:microservices-apple/*");

        var result = await searchParams.ExecuteAsync();

        ListResourcesResult response = new ListResourcesResult
        {
            Resources = result.Resources.Select(r => new Resource
            {
                PublicId = r.PublicId,
                Format = r.Format,
                DisplayName = r.DisplayName,
                SecureUrl = new Uri(r.SecureUrl),
                Length = r.Length,
                Bytes = r.Bytes,
                Width = r.Width,
                Height = r.Height,
            }).ToArray(),
            NextCursor = result.NextCursor,
        };

        return response;
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
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<DeletionResult> DeleteImageFileAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);

        var result = await _cloudinary.DestroyAsync(deleteParams);

        return result;
    }
}
