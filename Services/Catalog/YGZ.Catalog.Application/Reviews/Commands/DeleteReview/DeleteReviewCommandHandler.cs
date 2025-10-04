using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Ordering.Api.Protos;

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
        throw new NotImplementedException();
        //var userId = _userContext.GetUserId();

        //var filter = Builders<Review>.Filter.Eq(x => x.Id, ReviewId.Of(request.ReviewId));
        //filter &= Builders<Review>.Filter.Eq(x => x.CustomerId, _userContext.GetUserId());

        //var review = await _reviewRepository.GetByFilterAsync(filter, cancellationToken);

        //if (review is null)
        //{
        //    return Errors.Review.NotFound;
        //}

        //review.Delete();

        //var rpcResult = await _orderingProtoServiceClient.UpdateReviewOrderItemAsync(new UpdateReviewOrderItemRquest()
        //{
        //    ReviewId = review.Id.ToString(),
        //    OrderItemId = review.OrderItemId,
        //    CustomerId = userId,
        //    ReviewContent = "",
        //    ReviewStar = 1,
        //    IsReviewed = false,
        //});

        //if (rpcResult.IsFailure)
        //{
        //    return OrderingGrpcErrors.OrderingGrpc.CannotUpdateReviewOrderItem;
        //}

        //var result = await _reviewRepository.DeleteAsync(request.ReviewId, review, cancellationToken);

        //return result;
    }
}
