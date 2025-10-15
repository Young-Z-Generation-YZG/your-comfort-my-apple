

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Ordering.Application.Abstractions;
using YGZ.Ordering.Application.Abstractions.Data;

namespace YGZ.Ordering.Application.Orders.Commands.ConfirmOrder;

public class ConfirmOrderHandler : ICommandHandler<ConfirmOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRequestContext _userContext;

    public ConfirmOrderHandler(IOrderRepository orderRepository, IUserRequestContext userContext)
    {
        _orderRepository = orderRepository;
        _userContext = userContext;
    }

    public async Task<Result<bool>> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
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

        //if (order.Status != OrderStatus.PENDING)
        //{
        //    return Errors.Order.CannotConfirmOrder;
        //}

        //order.Confirm();

        //return await _orderRepository.UpdateAsync(order, cancellationToken);
    }
}
