using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
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

        // var rpcResult = await _orderingProtoServiceClient.UpdateReviewOrderItemAsync(new UpdateReviewOrderItemRquest
        // {
        //     ReviewId = string.IsNullOrWhiteSpace(review.Id.Value) ? null : StringValue.From(review.Id.Value),
        //     OrderItemId = string.IsNullOrWhiteSpace(review.CustomerOrder.OrderItemId) ? null : StringValue.From(review.CustomerOrder.OrderItemId),
        //     CustomerId = string.IsNullOrWhiteSpace(userId) ? null : StringValue.From(userId),
        //     ReviewContent = null,
        //     ReviewStar = null,
        //     IsReviewed = false
        // });

        //if (rpcResult.IsFailure)
        //{
        //    return Error.Failure(code: "OrderingGrpc.CannotUpdateReviewOrderItem", message: rpcResult.ErrorMessage?.Value ?? "Cannot update review order item", serviceName: "CatalogService");
        //}

        var result = await _reviewRepository.DeleteAsync(request.ReviewId, review, cancellationToken);

        return result;
    }
}
