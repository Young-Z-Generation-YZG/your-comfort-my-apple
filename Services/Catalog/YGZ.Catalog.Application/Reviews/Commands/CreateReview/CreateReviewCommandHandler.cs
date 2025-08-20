

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions;
using YGZ.Catalog.Application.Reviews.Extensions;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Ordering.Api.Protos;
using YGZ.BuildingBlocks.Shared.Errors.Common;

namespace YGZ.Catalog.Application.Reviews.Commands;

public class CreateReviewCommandHandler : ICommandHandler<CreateReviewCommand, bool>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserRequestContext _userContext;
    private readonly OrderingProtoService.OrderingProtoServiceClient _orderingProtoServiceClient;

    public CreateReviewCommandHandler(IReviewRepository reviewRepository, IUserRequestContext userContext, OrderingProtoService.OrderingProtoServiceClient orderingProtoServiceClient)
    {
        _reviewRepository = reviewRepository;
        _userContext = userContext;
        _orderingProtoServiceClient = orderingProtoServiceClient;
    }

    public async Task<Result<bool>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId();

        var review = request.ToEntity(userId);

        var result = await _reviewRepository.InsertOneAsync(review);

        if(result.IsFailure)
        {
            return result.Error;
        }

        var rpcResult = await _orderingProtoServiceClient.UpdateReviewOrderItemAsync(new UpdateReviewOrderItemRquest()
        {
            ReviewId = review.Id.ToString(),
            OrderItemId = request.OrderItemId,
            CustomerId = userId,
            ReviewContent = request.Content,
            ReviewStar = request.Rating,
            IsReviewed = true,
        });

        if(rpcResult.IsFailure)
        {
            return Errors.OrderingGrpc.CannotUpdateReviewOrderItem;
        }

        return true;
    }
}
