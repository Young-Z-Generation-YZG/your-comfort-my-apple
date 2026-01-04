using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone.Events;
using YGZ.Catalog.Domain.Products.ProductModels;
using YGZ.Ordering.Api.Protos;

namespace YGZ.Catalog.Application.Reviews.Events.DomainEvents;

public class ReviewCreatedDomainEventHandler : INotificationHandler<ReviewCreatedDomainEvent>
{
    private readonly ILogger<ReviewCreatedDomainEventHandler> _logger;
    private readonly IMongoRepository<ProductModel, ModelId> _productModelRepository;
    private readonly OrderingProtoService.OrderingProtoServiceClient _orderingProtoServiceClient;


    public ReviewCreatedDomainEventHandler(
        ILogger<ReviewCreatedDomainEventHandler> logger,
        IMongoRepository<ProductModel, ModelId> productModelRepository,
        OrderingProtoService.OrderingProtoServiceClient orderingProtoServiceClient)
    {
        _logger = logger;
        _productModelRepository = productModelRepository;
        _orderingProtoServiceClient = orderingProtoServiceClient;
    }

    public async Task Handle(ReviewCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var productModel = await _productModelRepository.GetByIdAsync(notification.Review.ModelId.Value!, cancellationToken, ignoreBaseFilter: true);

            productModel.AddNewRating(notification.Review);

            if (notification.Review.OrderInfo.OrderId != "SEED_DATA")
            {
                try
                {
                    var rpcResult = await _orderingProtoServiceClient.UpdateOrderItemIsReviewedGrpcAsync(new UpdateOrderItemIsReviewedGrpcRequest()
                    {
                        OrderItemId = notification.Review.OrderInfo.OrderItemId,
                        IsReviewed = true
                    });

                    if (rpcResult.IsFailure)
                    {
                        var parameters = new { ReviewId = notification.Review.Id.Value, ModelId = notification.Review.ModelId.Value, OrderItemId = notification.Review.OrderInfo.OrderItemId };
                        _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                            nameof(_orderingProtoServiceClient.UpdateOrderItemIsReviewedGrpcAsync), rpcResult.ErrorMessage ?? "Failed to update order item review status", parameters);

                        throw new Exception(rpcResult.ErrorMessage ?? "Unknown");
                    }


                }
                catch (RpcException ex)
                {
                    var parameters = new { ReviewId = notification.Review.Id.Value, ModelId = notification.Review.ModelId.Value, OrderItemId = notification.Review.OrderInfo.OrderItemId };
                    _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                        nameof(_orderingProtoServiceClient.UpdateOrderItemIsReviewedGrpcAsync), ex.Message, parameters);
                    throw;
                }
            }
            else
            {
                // Only update product model repository if RPC call succeeds
                await _productModelRepository.UpdateAsync(productModel.Id.Value!, productModel, ignoreBaseFilter: true);

                var successParameters = new { ReviewId = notification.Review.Id.Value, ModelId = notification.Review.ModelId.Value, OrderItemId = notification.Review.OrderInfo.OrderItemId };
                _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                    nameof(Handle), "Successfully created review and updated product model rating", successParameters);
            }
        }
        catch (Exception ex)
        {
            var parameters = new { ReviewId = notification.Review.Id.Value, ModelId = notification.Review.ModelId.Value, OrderItemId = notification.Review.OrderInfo?.OrderItemId };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);
            // throw;
        }
    }
}
