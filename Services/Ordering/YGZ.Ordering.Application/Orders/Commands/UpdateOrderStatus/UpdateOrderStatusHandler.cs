

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Ordering.Application.Abstractions;
using YGZ.Ordering.Application.Abstractions.Data;

namespace YGZ.Ordering.Application.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusHandler : ICommandHandler<UpdateOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRequestContext _userContext;

    public UpdateOrderStatusHandler(IOrderRepository orderRepository, IUserRequestContext userContext)
    {
        _orderRepository = orderRepository;
        _userContext = userContext;
    }

    public async Task<Result<bool>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        //var orderId = OrderId.Of(request.OrderId);

        //var order = await _orderRepository.GetOrderByIdWithInclude(orderId, (x) => x.OrderItems, cancellationToken: cancellationToken);

        //if (order is null)
        //{
        //    return Errors.Order.DoesNotExist;
        //}

        //switch (request.UpdateStatus)
        //{
        //    case nameof(OrderStatus.PREPARING):
        //    order.Prepare();
        //    break;
        //    case nameof(OrderStatus.DELIVERING):
        //    order.Deliver();
        //    break;
        //    case nameof(OrderStatus.DELIVERED):
        //    order.Delivered();
        //    break;
        //    default:
        //    return Errors.Order.InvalidOrderStatus;
        //}

        //return await _orderRepository.UpdateAsync(order, cancellationToken);
    }
}
