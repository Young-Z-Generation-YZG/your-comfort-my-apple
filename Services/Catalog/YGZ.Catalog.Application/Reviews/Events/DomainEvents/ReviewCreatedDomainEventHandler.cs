

using MassTransit;
using MediatR;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone.Events;
using YGZ.Catalog.Domain.Products.ProductModels;
using YGZ.Ordering.Api.Protos;

namespace YGZ.Catalog.Application.Reviews.Events.DomainEvents;

public class ReviewCreatedDomainEventHandler : INotificationHandler<ReviewCreatedDomainEvent>
{
    private readonly IMongoRepository<ProductModel, ModelId> _productModelRepository;
    private readonly OrderingProtoService.OrderingProtoServiceClient _orderingProtoServiceClient;


    public ReviewCreatedDomainEventHandler(IMongoRepository<ProductModel, ModelId> productModelRepository, OrderingProtoService.OrderingProtoServiceClient orderingProtoServiceClient)
    {
        _productModelRepository = productModelRepository;
        _orderingProtoServiceClient = orderingProtoServiceClient;
    }

    public async Task Handle(ReviewCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var productModel = await _productModelRepository.GetByIdAsync(notification.Review.ModelId.Value!, cancellationToken);

        productModel.AddNewRating(notification.Review);

        await _productModelRepository.UpdateAsync(productModel.Id.Value!, productModel);

                var rpcResult = await _orderingProtoServiceClient.UpdateOrderItemIsReviewedGrpcAsync(new UpdateOrderItemIsReviewedGrpcRequest()
        {
            OrderItemId = notification.Review.OrderInfo.OrderItemId,
            IsReviewed = true
        });

        if (rpcResult.IsFailure)
        {
            throw new Exception(rpcResult.ErrorMessage ?? "Unknown");
        }
    }
}
