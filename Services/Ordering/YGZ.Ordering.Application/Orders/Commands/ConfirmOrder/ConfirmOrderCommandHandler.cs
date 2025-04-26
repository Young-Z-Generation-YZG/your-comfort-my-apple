

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Core.Enums;
using YGZ.Ordering.Domain.Core.Errors;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Commands.ConfirmOrder;

public class ConfirmOrderCommandHandler : ICommandHandler<ConfirmOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public ConfirmOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<bool>> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(request.OrderId);

        var order = await _orderRepository.GetOrderByIdWithInclude(orderId, (o) => o.OrderItems, cancellationToken);

        if(order is null)
        {
            return Errors.Order.DoesNotExist;
        }

        if(order.Status != OrderStatus.PENDING)
        {
            return Errors.Order.CannotConfirmOrder;
        }

        order.Confirm();

        return await _orderRepository.UpdateAsync(order, cancellationToken);
    }
}
