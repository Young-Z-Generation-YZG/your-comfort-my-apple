using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Products;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Product;

namespace YGZ.Catalog.Application.Products.Queries.GetProductModels;

public class GetProductModelsHandler : IQueryHandler<GetProductModelsQuery, PaginationResponse<ProductModelResponse>>
{
    private readonly ILogger<GetProductModelsHandler> _logger;
    private readonly IMongoRepository<ProductModel, ModelId> _modelRepository;

    public GetProductModelsHandler(ILogger<GetProductModelsHandler> logger, IMongoRepository<ProductModel, ModelId> modelRepository)
    {
        _logger = logger;
        _modelRepository = modelRepository;
    }

    public async Task<Result<PaginationResponse<ProductModelResponse>>> Handle(GetProductModelsQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page ?? 1;
        var limit = request.Limit ?? 10;

        var filter = BuildFilter(request);

        var result = await _modelRepository.GetAllAsync(
            _page: page,
            _limit: limit,
            filter: filter,
            sort: null,
            cancellationToken: cancellationToken);

        var response = MapToResponse(result, request);

        return response;
    }

    private static FilterDefinition<ProductModel> BuildFilter(GetProductModelsQuery request)
    {
        var filterBuilder = Builders<ProductModel>.Filter;
        var filter = filterBuilder.Empty;

        if (!string.IsNullOrWhiteSpace(request.TextSearch))
        {
            // Search in name field (case-insensitive)
            filter &= filterBuilder.Regex("name", new MongoDB.Bson.BsonRegularExpression(request.TextSearch, "i"));
        }

        return filter;
    }

    private static PaginationResponse<ProductModelResponse> MapToResponse((List<ProductModel> items, int totalRecords, int totalPages) result, GetProductModelsQuery request)
    {
        var items = result.items.Select(model => model.ToResponse()).ToList();

        var links = PaginationLinksBuilder.Build(
            basePath: "",
            request: request,
            currentPage: request.Page ?? 1,
            totalPages: result.totalPages);

        return new PaginationResponse<ProductModelResponse>
        {
            TotalRecords = result.totalRecords,
            TotalPages = result.totalPages,
            PageSize = request.Limit ?? 10,
            CurrentPage = request.Page ?? 1,
            Links = links,
            Items = items
        };
    }
}
