using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Reviews;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Iphone.Entities;
using YGZ.Catalog.Domain.Products.Iphone.ValueObjects;

namespace YGZ.Catalog.Application.Reviews.Queries.GetReviewsByOrder;

public class GetReviewsByOrderHandler : IQueryHandler<GetReviewsByOrderQuery, List<ReviewInOrderResponse>>
{
    private readonly ILogger<GetReviewsByOrderHandler> _logger;
    private readonly IMongoRepository<Review, ReviewId> _reviewRepository;

    public GetReviewsByOrderHandler(IMongoRepository<Review, ReviewId> reviewRepository, ILogger<GetReviewsByOrderHandler> logger)
    {
        _reviewRepository = reviewRepository;
        _logger = logger;
    }

    public async Task<Result<List<ReviewInOrderResponse>>> Handle(GetReviewsByOrderQuery request, CancellationToken cancellationToken)
    {
        var filter = BuildFilter(request);

        var reviews = await _reviewRepository.GetAllAsync(filter, cancellationToken);

        var response = reviews.Select(r => r.ToReviewInOrderResponse()).ToList();

        return response;
    }

    private static FilterDefinition<Review> BuildFilter(GetReviewsByOrderQuery request)
    {
        var filterBuilder = Builders<Review>.Filter;
        var filter = filterBuilder.Eq(x => x.OrderInfo.OrderId, request.OrderId);

        // Exclude soft-deleted reviews
        filter &= filterBuilder.Eq(x => x.IsDeleted, false);

        return filter;
    }
}