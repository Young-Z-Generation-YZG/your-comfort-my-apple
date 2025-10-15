

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Ordering.Application.Abstractions;
using YGZ.Ordering.Application.Abstractions.Data;

namespace YGZ.Ordering.Application.Orders.Commands.CancelOrder;

public class CancelOrderHandler : ICommandHandler<CancelOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRequestContext _userContext;

    public CancelOrderHandler(IOrderRepository orderRepository, IUserRequestContext userContext)
    {
        _orderRepository = orderRepository;
        _userContext = userContext;
    }

    public async Task<Result<bool>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        //var orderId = OrderId.Of(request.OrderId);
        //var userId = UserId.Of(Guid.Parse(_userContext.GetUserId()));

        //var orders = await _orderRepository.GetUserOrdersWithItemAsync(userId, cancellationToken);

        //var order = orders.FirstOrDefault(x => x.Id == orderId);

        //if (order is null)
        //{
        //    return Errors.Order.DoesNotExist;
        //}

        //switch (order.Status.Name)
        //{
        //    case nameof(OrderStatus.PENDING):
        //    order.Cancel();
        //    break;
        //    case nameof(OrderStatus.CONFIRMED):
        //    order.Cancel();
        //    break;
        //    case nameof(OrderStatus.PREPARING):
        //    order.Cancel();
        //    break;
        //    default:
        //    return Errors.Order.CannotCancelOrder;
        //}

        //return await _orderRepository.UpdateAsync(order, cancellationToken);
    }
}
