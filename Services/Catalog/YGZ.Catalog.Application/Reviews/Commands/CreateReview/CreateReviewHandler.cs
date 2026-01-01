

using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Iphone.Entities;
using YGZ.Catalog.Domain.Products.Iphone.ValueObjects;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Reviews.Commands;

public class CreateReviewHandler : ICommandHandler<CreateReviewCommand, bool>
{
    private readonly ILogger<CreateReviewHandler> _logger;
    private readonly IMongoRepository<Review, ReviewId> _reviewRepository;
    private readonly IMongoRepository<SKU, SkuId> _skuRepository;
    private readonly IUserHttpContext _userHttpContext;

    public CreateReviewHandler(ILogger<CreateReviewHandler> logger,
                               IMongoRepository<Review, ReviewId> reviewRepository,
                               IMongoRepository<SKU, SkuId> skuRepository,
                               IUserHttpContext userHttpContext)
    {
        _logger = logger;
        _reviewRepository = reviewRepository;
        _skuRepository = skuRepository;
        _userHttpContext = userHttpContext;
    }

    public async Task<Result<bool>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var userId = _userHttpContext.GetUserId();
        var userEmail = _userHttpContext.GetUserEmail();

        var sku = await _skuRepository.GetByIdAsync(request.SkuId, cancellationToken);

        if (sku is null)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_skuRepository.GetByIdAsync), "SKU not found", new { skuId = request.SkuId, orderId = request.OrderId, orderItemId = request.OrderItemId });

            return false;
        }

        var orderInfo = OrderInfo.Create(
            orderId: request.OrderId,
            orderItemId: request.OrderItemId
        );

        var customerReviewInfo = CustomerReviewInfo.Create(
            name: userEmail ?? userId ?? "Anonymous",
            avatarImageUrl: null,
            userId: userId
        );

        var review = Review.Create(
            reviewId: ReviewId.Create(),
            modelId: sku.ModelId,
            skuId: sku.Id,
            orderInfo: orderInfo,
            customerReviewInfo: customerReviewInfo,
            content: request.Content,
            rating: request.Rating
        );

        var result = await _reviewRepository.InsertOneAsync(review);

        if (result.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_reviewRepository.InsertOneAsync), "Failed to create review", new { skuId = request.SkuId, orderId = request.OrderId, orderItemId = request.OrderItemId, rating = request.Rating, error = result.Error });

            return result.Error;
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully created review", new { reviewId = review.Id.ToString(), skuId = request.SkuId, orderId = request.OrderId, rating = request.Rating });

        return true;
    }
}
