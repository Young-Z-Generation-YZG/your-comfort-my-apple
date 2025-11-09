using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products.Iphone.Entities;
using YGZ.Catalog.Domain.Products.Iphone.ValueObjects;
using YGZ.Ordering.Api.Protos;

namespace YGZ.Catalog.Application.Reviews.Commands;

public class DeleteReviewHandler : ICommandHandler<DeleteReviewCommand, bool>
{
    private readonly IMongoRepository<Review, ReviewId> _reviewRepository;
    private readonly OrderingProtoService.OrderingProtoServiceClient _orderingProtoServiceClient;

    public DeleteReviewHandler(IMongoRepository<Review, ReviewId> reviewRepository, OrderingProtoService.OrderingProtoServiceClient orderingProtoServiceClient)
    {
        _reviewRepository = reviewRepository;
        _orderingProtoServiceClient = orderingProtoServiceClient;
    }

    public async Task<Result<bool>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {

        var filter = Builders<Review>.Filter.Eq(x => x.Id, ReviewId.Of(request.ReviewId));

        var review = await _reviewRepository.GetByFilterAsync(filter, cancellationToken);

        if (review is null)
        {
            return Errors.Review.NotFound;
        }

        review.Delete();

        var rpcResult = await _orderingProtoServiceClient.UpdateOrderItemIsReviewedGrpcAsync(new UpdateOrderItemIsReviewedGrpcRequest
        {
            OrderItemId = review.OrderInfo.OrderItemId,
            IsReviewed = false
        });

        if (rpcResult.IsFailure)
        {
            return Result<bool>.Failure(Error.GrpcError(rpcResult.ErrorCode ?? "Unknown", rpcResult.ErrorMessage ?? "Unknown"));
        }

        var result = await _reviewRepository.DeleteAsync(request.ReviewId, review, cancellationToken);

        return result;
    }
}
