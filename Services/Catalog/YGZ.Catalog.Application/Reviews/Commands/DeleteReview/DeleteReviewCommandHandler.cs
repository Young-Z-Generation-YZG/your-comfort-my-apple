

using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;
using YGZ.Ordering.Api.Protos;
using OrderingGrpcErrors = YGZ.BuildingBlocks.Shared.Errors.Common.Errors;

namespace YGZ.Catalog.Application.Reviews.Commands;

public class DeleteReviewCommandHandler : ICommandHandler<DeleteReviewCommand, bool>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserRequestContext _userContext;
    private readonly OrderingProtoService.OrderingProtoServiceClient _orderingProtoServiceClient;
    public DeleteReviewCommandHandler(IReviewRepository reviewRepository, IUserRequestContext userContext, OrderingProtoService.OrderingProtoServiceClient orderingProtoServiceClient)
    {
        _reviewRepository = reviewRepository;
        _userContext = userContext;
        _orderingProtoServiceClient = orderingProtoServiceClient;
    }
    public async Task<Result<bool>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId();

        var filter = Builders<Review>.Filter.Eq(x => x.Id, ReviewId.Of(request.ReviewId));
        filter &= Builders<Review>.Filter.Eq(x => x.CustomerId, _userContext.GetUserId());

        var review = await _reviewRepository.GetByFilterAsync(filter, cancellationToken);

        if (review is null)
        {
            return Errors.Review.NotFound;
        }

        review.Delete();

        var rpcResult = await _orderingProtoServiceClient.UpdateReviewOrderItemAsync(new UpdateReviewOrderItemRquest()
        {
            ReviewId = review.Id.ToString(),
            OrderItemId = review.OrderItemId,
            CustomerId = userId,
            ReviewContent = "",
            ReviewStar = 1,
            IsReviewed = false,
        });

        if (rpcResult.IsFailure)
        {
            return OrderingGrpcErrors.OrderingGrpc.CannotUpdateReviewOrderItem;
        }

        var result = await _reviewRepository.DeleteAsync(request.ReviewId, review, cancellationToken);

        return result;
    }
}
