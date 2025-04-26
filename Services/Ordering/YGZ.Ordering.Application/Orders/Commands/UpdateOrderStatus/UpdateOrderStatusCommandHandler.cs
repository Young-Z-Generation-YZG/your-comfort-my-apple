

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Core.Enums;
using YGZ.Ordering.Domain.Core.Errors;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandHandler : ICommandHandler<UpdateOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }


    public async Task<Result<bool>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(request.OrderId);

        var order = await _orderRepository.GetOrderByIdWithInclude(orderId, (x) => x.OrderItems, cancellationToken: cancellationToken);

        if(order is null)
        {
            return Errors.Order.DoesNotExist;
        }

        switch(request.UpdatedStatus)
        {
            case nameof(OrderStatus.PREPARING):
                order.Prepare();
                break;
            case nameof(OrderStatus.DELIVERING):
                order.Deliver();
                break;
            case nameof(OrderStatus.DELIVERED):
                order.Complete();
                break;
            default:
                return Errors.Order.InvalidOrderStatus;
        }

        return await _orderRepository.UpdateAsync(order, cancellationToken);
    }
}
