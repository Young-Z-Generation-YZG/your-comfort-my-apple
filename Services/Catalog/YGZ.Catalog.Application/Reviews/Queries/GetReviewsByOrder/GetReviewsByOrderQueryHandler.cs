

using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Reviews;
using YGZ.Catalog.Application.Abstractions;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;

namespace YGZ.Catalog.Application.Reviews.Queries.GetReviewsByOrder;

public class GetReviewsByOrderQueryHandler : IQueryHandler<GetReviewsByOrderQuery, List<ReviewInOrderResponse>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserRequestContext _userContext;
    private readonly ILogger<GetReviewsByOrderQueryHandler> _logger;

    public GetReviewsByOrderQueryHandler(IReviewRepository reviewRepository, ILogger<GetReviewsByOrderQueryHandler> logger, IUserRequestContext userContext)
    {
        _reviewRepository = reviewRepository;
        _logger = logger;
        _userContext = userContext;
    }
    public async Task<Result<List<ReviewInOrderResponse>>> Handle(GetReviewsByOrderQuery request, CancellationToken cancellationToken)
    {
        var filter = Builders<Review>.Filter.Eq(x => x.OrderId, request.OrderId);
        filter &= Builders<Review>.Filter.Eq(x => x.CustomerId, _userContext.GetUserId());

        var result = await _reviewRepository.GetAllAsync(filter, cancellationToken);

        List<ReviewInOrderResponse> response = MapToResponse(result);

        return response;
    }

    private List<ReviewInOrderResponse> MapToResponse(List<Review> result)
    {
        return result.Select(r => new ReviewInOrderResponse
        {
            ReviewId = r.Id.Value!,
            ProductId = r.ProductId.Value!,
            ModelId = r.ModelId.Value!,
            OrderId = r.OrderId,
            OrderItemId = r.OrderItemId,
            Rating = r.Rating,
            Content = r.Content,
            CreatedAt = r.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
        }).ToList();
    }
}