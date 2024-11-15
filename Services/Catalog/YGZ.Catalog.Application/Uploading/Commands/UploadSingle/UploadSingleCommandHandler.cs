

using CloudinaryDotNet.Actions;
using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Application.Core.Abstractions.Uploading;
using YGZ.Catalog.Domain.Core.Abstractions.Result;

namespace YGZ.Catalog.Application.Uploading.Commands.UploadSingle;

public class UploadSingleCommandHandler : ICommandHandler<UploadSingleCommand, ImageUploadResult>
{
    private readonly IUploadService _uploadService;

    public UploadSingleCommandHandler(IUploadService uploadService)
    {
        _uploadService = uploadService;
    }

    public async Task<Result<ImageUploadResult>> Handle(UploadSingleCommand request, CancellationToken cancellationToken)
    {
        var result = await _uploadService.UploadImageFileAsync(request.File).ConfigureAwait(false);

        Console.WriteLine(result);

        return result;
    }
}
