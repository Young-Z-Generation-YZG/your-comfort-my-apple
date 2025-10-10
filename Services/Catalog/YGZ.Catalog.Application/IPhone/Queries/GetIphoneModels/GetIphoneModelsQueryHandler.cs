using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone;

namespace YGZ.Catalog.Application.IPhone.Queries.GetIphoneModels;

public class GetModelsQueryHandler : IQueryHandler<GetIphoneModelsQuery, PaginationResponse<IphoneModelResponse>>
{
    private readonly IMongoRepository<IphoneModel, ModelId> _modelRepository;
    private readonly ILogger<GetModelsQueryHandler> _logger;

    public GetModelsQueryHandler(IMongoRepository<IphoneModel, ModelId> modelRepository,
                                 ILogger<GetModelsQueryHandler> logger)
    {
        _modelRepository = modelRepository;
        _logger = logger;
    }

    public async Task<Result<PaginationResponse<IphoneModelResponse>>> Handle(GetIphoneModelsQuery request, CancellationToken cancellationToken)
    {

        var (filter, sort) = GetFilterDefinition(request);

        var allModels = await _modelRepository.GetAllAsync(request.Page, request.Limit, filter, null, cancellationToken);

        PaginationResponse<IphoneModelResponse> response = MapToResponse(allModels);

        return response;
    }

    private PaginationResponse<IphoneModelResponse> MapToResponse((List<IphoneModel> items, int totalRecords, int totalPages) allModels)
    {
        var items = allModels.items.Select(model => new IphoneModelResponse
        {
            Id = model.Id.Value!,
            Category = model.Category.ToResponse(),
            Name = model.Name,
            ModelItems = model.Models.Select(m => m.ToResponse()).ToList(),
            ColorItems = model.Colors.Select(c => c.ToResponse()).ToList(),
            StorageItems = model.Storages.Select(s => s.ToResponse()).ToList(),
            SkuPrices = model.Prices.Select(p => p.ToResponse()).ToList(),
            Description = model.Description,
            ShowcaseImages = model.ShowcaseImages.Select(img => img.ToResponse()).ToList(),
            OverallSold = model.OverallSold,
            AverageRating = new AverageRatingResponse
            {
                RatingAverageValue = model.AverageRating.RatingAverageValue,
                RatingCount = model.AverageRating.RatingCount
            },
            RatingStars = model.RatingStars.Select(s => new RatingStarResponse
            {
                Star = s.Star,
                Count = s.Count
            }).ToList(),
            Slug = model.Slug.Value!
        }).ToList();

        return new PaginationResponse<IphoneModelResponse>
        {
            TotalRecords = allModels.totalRecords,
            TotalPages = allModels.totalPages,
            PageSize = items.Count,
            CurrentPage = 1,
            Links = new PaginationLinks("", "", "", ""),
            Items = items
        };
    }

    private (FilterDefinition<IphoneModel> filterBuilder, SortDefinition<IphoneModel> sort) GetFilterDefinition(GetIphoneModelsQuery request)
    {
        var filterBuilder = Builders<IphoneModel>.Filter;
        var filter = filterBuilder.Empty;

        if (request.Colors is not null && request.Colors.Any())
        {
            var productColors = request.Colors.ToList();

            filter &= filterBuilder.AnyIn(x => x.Colors.Select(c => c.NormalizedName), productColors);
        }

        if (request.Storages is not null && request.Storages.Any())
        {
            var productStorages = request.Storages.ToList();

            filter &= filterBuilder.AnyIn(x => x.Storages.Select(s => s.NormalizedName), productStorages);
        }

        if (request.Models is not null && request.Models.Any())
        {
            var productModels = request.Models.Select(x => x.ToLower()).ToList();
            filter &= filterBuilder.AnyIn(x => x.Models.Select(m => m.NormalizedName), productModels);
        }

        if (!string.IsNullOrEmpty(request.MinPrice))
        {
            decimal.TryParse(request.MinPrice, out var priceFromValue);
            if (priceFromValue > 0)
            {
                filter &= filterBuilder.ElemMatch(x => x.Prices, p => p.UnitPrice >= priceFromValue);
            }
        }

        if (!string.IsNullOrEmpty(request.MaxPrice))
        {
            decimal.TryParse(request.MaxPrice, out var priceToValue);
            if (priceToValue > 0)
            {
                filter &= filterBuilder.ElemMatch(x => x.Prices, p => p.UnitPrice <= priceToValue);
            }
        }

        var sortBuilder = Builders<IphoneModel>.Sort;
        var sort = sortBuilder.Ascending("prices.unit_price"); // default sort by minimum price

        if (!string.IsNullOrEmpty(request.PriceSort))
        {
            sort = request.PriceSort.ToUpper() switch
            {
                "ASC" => sortBuilder.Ascending("prices.unit_price"),
                "DESC" => sortBuilder.Descending("prices.unit_price"),
                _ => sortBuilder.Ascending("prices.unit_price"),
            };
        }

        return (filter, sort);
    }
}