

using CloudinaryDotNet.Actions;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Cloudinary.Queries.GetImages;

public sealed record GetImagesQuery : IQuery<ListResourcesResult> { }
