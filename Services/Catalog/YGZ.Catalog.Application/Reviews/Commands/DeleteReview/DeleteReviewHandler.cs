using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products.Iphone.Entities;
using YGZ.Catalog.Domain.Products.Iphone.ValueObjects;
using YGZ.Ordering.Api.Protos;
using Grpc.Core;

namespace YGZ.Catalog.Application.Reviews.Commands;

public class DeleteReviewHandler : ICommandHandler<DeleteReviewCommand, bool>
{
    private readonly ILogger<DeleteReviewHandler> _logger;
    private readonly IMongoRepository<Review, ReviewId> _reviewRepository;
    private readonly OrderingProtoService.OrderingProtoServiceClient _orderingProtoServiceClient;

    public DeleteReviewHandler(IMongoRepository<Review, ReviewId> reviewRepository, OrderingProtoService.OrderingProtoServiceClient orderingProtoServiceClient, ILogger<DeleteReviewHandler> logger)
    {
        _reviewRepository = reviewRepository;
        _orderingProtoServiceClient = orderingProtoServiceClient;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {

        var filter = Builders<Review>.Filter.Eq(x => x.Id, ReviewId.Of(request.ReviewId));

        var review = await _reviewRepository.GetByFilterAsync(filter, cancellationToken, ignoreBaseFilter: true);

        if (review is null)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_reviewRepository.GetByFilterAsync), "Review not found", new { reviewId = request.ReviewId });

            return Errors.Review.NotFound;
        }

        review.Delete();

        try
        {
            var rpcResult = await _orderingProtoServiceClient.UpdateOrderItemIsReviewedGrpcAsync(new UpdateOrderItemIsReviewedGrpcRequest
            {
                OrderItemId = review.OrderInfo.OrderItemId,
                IsReviewed = false
            }, cancellationToken: cancellationToken);

            if (rpcResult.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_orderingProtoServiceClient.UpdateOrderItemIsReviewedGrpcAsync), "Failed to update order item reviewed status", new { reviewId = request.ReviewId, orderItemId = review.OrderInfo.OrderItemId, errorCode = rpcResult.ErrorCode, errorMessage = rpcResult.ErrorMessage });

                return Result<bool>.Failure(Error.GrpcError(rpcResult.ErrorCode ?? "Unknown", rpcResult.ErrorMessage ?? "Unknown"));
            }
        }
        catch (RpcException ex)
        {
            var parameters = new { reviewId = request.ReviewId, orderItemId = review.OrderInfo.OrderItemId };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_orderingProtoServiceClient.UpdateOrderItemIsReviewedGrpcAsync), ex.Message, parameters);
            throw;
        }

        var result = await _reviewRepository.DeleteAsync(request.ReviewId, review, cancellationToken, ignoreBaseFilter: true);

        if (result.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_reviewRepository.DeleteAsync), "Failed to delete review", new { reviewId = request.ReviewId, error = result.Error });

            return result.Error;
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully deleted review", new { reviewId = request.ReviewId, orderItemId = review.OrderInfo.OrderItemId });

        return result;
    }
}
