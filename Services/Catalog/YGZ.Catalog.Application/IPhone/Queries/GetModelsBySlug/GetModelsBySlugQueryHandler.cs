
//using Microsoft.Extensions.Logging;
//using MongoDB.Driver;
//using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
//using YGZ.BuildingBlocks.Shared.Abstractions.Result;
//using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
//using YGZ.BuildingBlocks.Shared.Contracts.Common;
//using YGZ.Catalog.Application.IPhone16.Queries.GetIPhonesByModelSlug;
//using YGZ.Catalog.Application.Abstractions.Data;
//using YGZ.Catalog.Domain.Core.Errors;
//using YGZ.Catalog.Domain.Products.Common.ValueObjects;
//using YGZ.Catalog.Domain.Products.Iphone16;
//using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

//namespace YGZ.Catalog.Application.IPhone16.Queries.GetIPhone16Models;

//public class GetModelsBySlugQueryHandler : IQueryHandler<GetModelsBySlugQuery, IPhoneModelResponse>
//{
//    private readonly IMongoRepository<IPhone16Model, IPhone16ModelId> _modelRepository;
//    private readonly ILogger<GetIPhonesByModelSlugQueryHandler> _logger;

//    public GetModelsBySlugQueryHandler(IMongoRepository<IPhone16Model, IPhone16ModelId> modelRepository, ILogger<GetIPhonesByModelSlugQueryHandler> logger)
//    {
//        _modelRepository = modelRepository;
//        _logger = logger;
//    }

//    public async Task<Result<IPhoneModelResponse>> Handle(GetModelsBySlugQuery request, CancellationToken cancellationToken)
//    {
//        var filter = Builders<IPhone16Model>.Filter.Eq(x => x.Slug, Slug.Of(request.ModelSlug));

//        var result = await _modelRepository.GetAllAsync(filter, cancellationToken);

//        if (result is null)
//        {
//            return Errors.Category.DoesNotExist;
//        }

//        IPhoneModelResponse response = MapToResponse(result);

//        return response;
//    }

//    private IPhoneModelResponse MapToResponse(List<IPhone16Model> result)
//    {
//        return new IPhoneModelResponse()
//        { 
//            ModelId = result.First().Id.Value!,
//            ModelName = result.First().Name,
//            ModelItems = result.First().Models.Select(x => new ModelItemResponse
//            {
//                ModelName = x.ModelName,
//                ModelOrder = (int)x.ModelOrder!,
//            }).ToList(),
//            ColorItems = result.First().Colors.Select(x => new ColorResponse
//            {
//                ColorName = x.ColorName,
//                ColorHex = x.ColorHex,
//                ColorImage = x.ColorImage,
//                ColorOrder = x.ColorOrder,
//            }).ToList(),
//            StorageItems = result.First().Storages.Select(x => new StorageResponse
//            {
//                StorageName = x.Name,
//                StorageValue = x.Value,
//            }).ToList(),
//            GeneralModel = result.First().GeneralModel,
//            ModelDescription = result.First().Description,
//            OverallSold = result.First().OverallSold,
//            AverageRating = new AverageRatingResponse
//            {
//                RatingAverageValue = result.First().AverageRating.RatingAverageValue,
//                RatingCount = result.First().AverageRating.RatingCount,
//            },
//            RatingStars = result.First().RatingStars.Select(x => new RatingStarResponse
//            {
//                Star = x.Star,
//                Count = x.Count,
//            }).ToList(),
//            ModelImages = result.First().DescriptionImages.Select(x => new ImageResponse
//            {
//                ImageId = x.ImageId,
//                ImageUrl = x.ImageUrl,
//                ImageName = x.ImageName,
//                ImageDescription = x.ImageDescription,
//                ImageWidth = x.ImageWidth,
//                ImageHeight = x.ImageHeight,
//                ImageBytes = x.ImageBytes,
//                ImageOrder = x.ImageOrder,
//            }).ToList(),
//            ModelSlug = result.First().Slug.Value!,
//            CategoryId = result.First().CategoryId?.Value ?? string.Empty,
//            IsDeleted = result.First().IsDeleted,
//            DeletedBy = result.First().DeletedBy?.ToString(),
//            CreatedAt = result.First().CreatedAt,
//            UpdatedAt = result.First().UpdatedAt,
//            DeletedAt = result.First().DeletedAt,
//        };
//    }
//}