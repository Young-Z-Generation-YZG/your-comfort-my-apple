using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Reviews;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone.Entities;
using YGZ.Catalog.Domain.Products.Iphone.ValueObjects;
using YGZ.Catalog.Domain.Products.ProductModels;

namespace YGZ.Catalog.Application.Reviews.Queries.GetReviewsByProductModelId;

public class GetReviewsByProductModelIdHandler : IQueryHandler<GetReviewsByProductModelIdQuery, PaginationResponse<ProductModelReviewResponse>>
{
    private readonly ILogger<GetReviewsByProductModelIdHandler> _logger;
    private readonly IMongoRepository<Review, ReviewId> _reviewRepository;
    private readonly IMongoRepository<ProductModel, ModelId> _productModelRepository;

    public GetReviewsByProductModelIdHandler(ILogger<GetReviewsByProductModelIdHandler> logger, IMongoRepository<Review, ReviewId> reviewRepository, IMongoRepository<ProductModel, ModelId> productModelRepository)
    {
        _logger = logger;
        _reviewRepository = reviewRepository;
        _productModelRepository = productModelRepository;
    }

    public async Task<Result<PaginationResponse<ProductModelReviewResponse>>> Handle(GetReviewsByProductModelIdQuery request, CancellationToken cancellationToken)
    {
        var productModel = await _productModelRepository.GetByIdAsync(request.ProductModelId, cancellationToken);

        if (productModel is null)
        {
            return Errors.ProductModel.DoesNotExist;
        }

        var filter = BuildFilter(request, productModel);
        var sort = BuildSort(request);

        var result = await _reviewRepository.GetAllAsync(
            _page: request.Page,
            _limit: request.Limit,
            filter: filter,
            sort: sort,
            cancellationToken: cancellationToken);

        var response = MapToResponse(result, request);

        return response;
    }

    private static FilterDefinition<Review> BuildFilter(GetReviewsByProductModelIdQuery request, ProductModel productModel)
    {
        var filterBuilder = Builders<Review>.Filter;
        var filter = filterBuilder.Empty;

        filter &= filterBuilder.Eq(x => x.ModelId, productModel.Id);
        filter &= filterBuilder.Eq(x => x.IsDeleted, false);

        return filter;
    }

    private static SortDefinition<Review> BuildSort(GetReviewsByProductModelIdQuery request)
    {
        var sortBuilder = Builders<Review>.Sort;

        var sort = sortBuilder.Descending(x => x.Rating).Descending(x => x.CreatedAt);

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            var sortOrder = request.SortOrder?.ToUpper() ?? "DESC";
            var sortBy = request.SortBy.ToUpper();

            sort = sortBy switch
            {
                "RATING" => sortOrder == "ASC"
                    ? sortBuilder.Ascending(x => x.Rating).Descending(x => x.CreatedAt)
                    : sortBuilder.Descending(x => x.Rating).Descending(x => x.CreatedAt),
                "CREATED_AT" or "CREATED_AT" => sortOrder == "ASC"
                    ? sortBuilder.Ascending(x => x.CreatedAt)
                    : sortBuilder.Descending(x => x.CreatedAt),
                _ => sortBuilder.Descending(x => x.Rating).Descending(x => x.CreatedAt)
            };
        }

        return sort;
    }

    private static PaginationResponse<ProductModelReviewResponse> MapToResponse(
        (List<Review> items, int totalRecords, int totalPages) result,
        GetReviewsByProductModelIdQuery request)
    {
        var reviewResponses = result.items.Select(r => r.ToProductModelReviewResponse()).ToList();

        var links = PaginationLinksBuilder.Build(
            basePath: "",
            request: request,
            currentPage: request.Page,
            totalPages: result.totalPages);

        return new PaginationResponse<ProductModelReviewResponse>
        {
            TotalRecords = result.totalRecords,
            TotalPages = result.totalPages,
            PageSize = request.Limit ?? 10,
            CurrentPage = request.Page ?? 1,
            Items = reviewResponses,
            Links = links
        };
    }
}
