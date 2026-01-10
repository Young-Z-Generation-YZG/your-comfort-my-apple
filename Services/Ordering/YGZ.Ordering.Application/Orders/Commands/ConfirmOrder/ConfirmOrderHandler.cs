

using System.Linq.Expressions;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Api.Protos;
using YGZ.Discount.Grpc.Protos;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Core.Errors;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Commands.ConfirmOrder;

public class ConfirmOrderHandler : ICommandHandler<ConfirmOrderCommand, bool>
{
    private readonly ILogger<ConfirmOrderHandler> _logger;
    private readonly IGenericRepository<Order, OrderId> _repository;
    private readonly CatalogProtoService.CatalogProtoServiceClient _catalogProtoServiceClient;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public ConfirmOrderHandler(ILogger<ConfirmOrderHandler> logger,
                               IGenericRepository<Order, OrderId> repository,
                               CatalogProtoService.CatalogProtoServiceClient catalogProtoServiceClient,
                               DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _logger = logger;
        _repository = repository;
        _catalogProtoServiceClient = catalogProtoServiceClient;
        _discountProtoServiceClient = discountProtoServiceClient;
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

            SkuModel? skuGrpc = null;

            try
            {
                skuGrpc = await _catalogProtoServiceClient.GetSkuByIdGrpcAsync(new GetSkuByIdRequest
                {
                    SkuId = item.SkuId
                }, cancellationToken: cancellationToken);
            }
            catch (RpcException ex)
            {
                throw;
            }


            if (skuGrpc is null)
            {
                throw new InvalidOperationException("SKU not found");
            }

            if (promotionType.Name == EPromotionType.EVENT_ITEM.Name)
            {
                EventItemModel? eventItemGrpc = null;

                try
                {
                    eventItemGrpc = await _discountProtoServiceClient.GetEventItemByIdGrpcAsync(new GetEventItemByIdRequest
                    {
                        EventItemId = item.PromotionId
                    }, cancellationToken: cancellationToken);
                }
                catch (RpcException ex)
                {
                    throw;
                }

                if (eventItemGrpc is null)
                {

                    throw new InvalidOperationException("Event item not found");
                }


                if (skuGrpc.ReservedForEvent is null)
                {
                    throw new InvalidOperationException("SKU is not reserved for event");
                }

                var availableQuantitySku = skuGrpc.ReservedForEvent.ReservedQuantity;
                var availableQuantityEventItem = eventItemGrpc.Stock - eventItemGrpc.Sold;

                if (availableQuantitySku <= 0 || availableQuantityEventItem <= 0 || (availableQuantitySku != availableQuantityEventItem))
                {
                    return Errors.Order.InsufficientStock;
                }

            }
        }

        if (order.OrderStatus != EOrderStatus.PENDING)
        {
            return Errors.Order.CannotConfirmOrder;
        }

        order.SetConfirmed();

        var updateResult = await _repository.UpdateAsync(order, cancellationToken);

        if (updateResult.IsFailure)
        {

            return updateResult.Error;
        }

        return updateResult;
    }
}
