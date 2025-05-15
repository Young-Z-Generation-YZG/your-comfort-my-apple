

using CloudinaryDotNet.Actions;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions.Uploading;

namespace YGZ.Catalog.Application.Cloudinary.Queries.GetImages;

public class GetImagesQueryHandler : IQueryHandler<GetImagesQuery, ListResourcesResult>
{
    private readonly IUploadImageService _uploadImageService;

    public GetImagesQueryHandler(IUploadImageService uploadImageService)
    {
        _uploadImageService = uploadImageService;
    }

    public async Task<Result<ListResourcesResult>> Handle(GetImagesQuery request, CancellationToken cancellationToken)
    {
        var result = await _uploadImageService.GetImages();

        return result;
    }
}
