
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Reviews;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

namespace YGZ.Catalog.Application.Reviews.Commands;

public class GetReviewsByModelQueryHandler : IQueryHandler<GetReviewsByModelQuery, PaginationResponse<ProductReviewResponse>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ILogger<GetReviewsByModelQueryHandler> _logger;

    public GetReviewsByModelQueryHandler(IReviewRepository reviewRepository, ILogger<GetReviewsByModelQueryHandler> logger)
    {
        _reviewRepository = reviewRepository;
        _logger = logger;
    }

    public async Task<Result<PaginationResponse<ProductReviewResponse>>> Handle(GetReviewsByModelQuery request, CancellationToken cancellationToken)
    {
        var filter = Builders<Review>.Filter.Eq(x => x.ModelId, IPhone16ModelId.Of(request.ModelId));
        var sort = Builders<Review>.Sort.Descending(x => x.Rating);

        var result = await _reviewRepository.GetAllAsync(request.Page, request.Limit, filter, sort, cancellationToken);

        var response = MapToResponse(result.items, result.totalRecords, result.totalPages, request);

        return response;
    }

    private PaginationResponse<ProductReviewResponse> MapToResponse(List<Review> reviews, int totalRecords, int totalPages, GetReviewsByModelQuery request)
    {
        var queryParams = QueryParamBuilder.Build(request);

        var links = PaginationLinksBuilder.Build(basePath: "/api/v1/products",
                                                 queryParams: queryParams,
                                                 currentPage: request.Page ?? 1,
                                                 totalPages: totalPages);

        var reviewResponses = reviews.Select(r => new ProductReviewResponse
        {
            ReviewId = r.Id.Value!,
            CustomerUserName = r.CustomerUserName,
            CustomerImage = "",
            Rating = r.Rating,
            Content = r.Content,
            CreatedAt = r.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
        }).ToList();

        var response = new PaginationResponse<ProductReviewResponse>
        {
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            PageSize = request.Limit ?? 10,
            CurrentPage = request.Page ?? 1,
            Items = reviewResponses,
            Links = links
        };

        return response;
    }
}