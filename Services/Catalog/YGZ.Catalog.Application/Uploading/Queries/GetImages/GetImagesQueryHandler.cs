

using CloudinaryDotNet.Actions;
using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Application.Core.Abstractions.Uploading;
using YGZ.Catalog.Domain.Core.Abstractions.Result;

namespace YGZ.Catalog.Application.Uploading.Queries.GetImages;

public class GetImagesQueryHandler : IQueryHandler<GetImagesQuery, ListResourcesResult>
{
    private readonly IUploadService _uploadService;

    public GetImagesQueryHandler(IUploadService uploadService)
    {
        _uploadService = uploadService;
    }

    public async Task<Result<ListResourcesResult>> Handle(GetImagesQuery request, CancellationToken cancellationToken)
    {
        var reuslt = await _uploadService.GetImages();

        return reuslt;
    }
}
