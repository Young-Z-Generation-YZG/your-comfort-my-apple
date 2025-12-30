

using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Api.Protos;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Core.Errors;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Commands.ConfirmOrder;

public class ConfirmOrderHandler : ICommandHandler<ConfirmOrderCommand, bool>
{
    private readonly ILogger<ConfirmOrderHandler> _logger;
    private readonly IGenericRepository<Order, OrderId> _repository;
    private readonly CatalogProtoService.CatalogProtoServiceClient _catalogProtoServiceClient;

    public ConfirmOrderHandler(ILogger<ConfirmOrderHandler> logger,
                               IGenericRepository<Order, OrderId> repository,
                               CatalogProtoService.CatalogProtoServiceClient catalogProtoServiceClient)
    {
        _logger = logger;
        _repository = repository;
        _catalogProtoServiceClient = catalogProtoServiceClient;
    }

    public async Task<Result<bool>> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(request.OrderId);

        var expressions = new Expression<Func<Order, object>>[]
        {
            x => x.OrderItems
        };

        var order = await _repository.GetByIdAsync(orderId, includes: expressions, cancellationToken: cancellationToken);

        if (order is null)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_repository.GetByIdAsync), "Order not found", new { orderId = request.OrderId });

            return Errors.Order.DoesNotExist;
        }

        foreach (var item in order.OrderItems)
        {
            EPromotionType promotionType = EPromotionType.UNKNOWN;

            if (!string.IsNullOrWhiteSpace(item.PromotionType))
            {
                EPromotionType.TryFromName(item.PromotionType, out var parsedType);
                promotionType = parsedType ?? EPromotionType.UNKNOWN;
            }

            var promotionTypeGrpc = promotionType.Name switch
            {
                "COUPON" => EPromotionTypeGrpc.PromotionTypeCoupon,
                "EVENT_ITEM" => EPromotionTypeGrpc.PromotionTypeEventItem,
                _ => EPromotionTypeGrpc.PromotionTypeUnknown
            };

            var result = await _catalogProtoServiceClient.CheckInsufficientGrpcAsync(new CheckInsufficientRequest
            {
                SkuId = item.SkuId,
                Quantity = item.Quantity,
                PromotionId = item.PromotionId ?? string.Empty,
                PromotionType = promotionTypeGrpc
            });

            if (!result.IsSuccess)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_catalogProtoServiceClient.CheckInsufficientGrpcAsync), "Insufficient stock for order item", new { orderId = request.OrderId, skuId = item.SkuId, quantity = item.Quantity, promotionId = item.PromotionId });

                return Errors.Order.InsufficientStock;
            }
        }

        if (order.OrderStatus != EOrderStatus.PENDING)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Cannot confirm order - order is not in PENDING status", new { orderId = request.OrderId, currentStatus = order.OrderStatus.Name });

            return Errors.Order.CannotConfirmOrder;
        }

        order.SetConfirmed();

        var updateResult = await _repository.UpdateAsync(order, cancellationToken);

        if (updateResult.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_repository.UpdateAsync), "Failed to update order status to confirmed", new { orderId = request.OrderId, error = updateResult.Error });

            return updateResult.Error;
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully confirmed order", new { orderId = request.OrderId, orderItemCount = order.OrderItems.Count });

        return updateResult;
    }
}
