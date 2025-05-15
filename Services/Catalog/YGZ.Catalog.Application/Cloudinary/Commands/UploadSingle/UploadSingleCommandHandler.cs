
using CloudinaryDotNet.Actions;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions.Uploading;

namespace YGZ.Catalog.Application.Cloudinary.Commands.UploadSingle;

public class UploadSingleCommandHandler : ICommandHandler<UploadSingleCommand, ImageUploadResult>
{
    private readonly IUploadImageService _uploadImageService;

    public UploadSingleCommandHandler(IUploadImageService uploadImageService)
    {
        _uploadImageService = uploadImageService;
    }

    public async Task<Result<ImageUploadResult>> Handle(UploadSingleCommand request, CancellationToken cancellationToken)
    {
        var result = await _uploadImageService.UploadImageFileAsync(request.File).ConfigureAwait(false);

        Console.WriteLine(result);

        return result;
    }
}
