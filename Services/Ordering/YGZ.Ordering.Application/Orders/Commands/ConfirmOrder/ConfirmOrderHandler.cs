

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
                return Errors.Order.InsufficientStock;
            }
        }

        if (order.OrderStatus != EOrderStatus.PENDING)
        {
            return Errors.Order.CannotConfirmOrder;
        }

        order.SetConfirmed();

        return await _repository.UpdateAsync(order, cancellationToken);
    }
}
