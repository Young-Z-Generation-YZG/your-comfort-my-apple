

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Ordering.Application.Abstractions;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Core.Enums;
using YGZ.Ordering.Domain.Core.Errors;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Commands.ConfirmOrder;

public class ConfirmOrderCommandHandler : ICommandHandler<ConfirmOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRequestContext _userContext;

    public ConfirmOrderCommandHandler(IOrderRepository orderRepository, IUserRequestContext userContext)
    {
        _orderRepository = orderRepository;
        _userContext = userContext;
    }

    public async Task<Result<bool>> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(request.OrderId);
        var userId = UserId.Of(Guid.Parse(_userContext.GetUserId()));

        var orders = await _orderRepository.GetUserOrdersWithItemAsync(userId, cancellationToken);

        var order = orders.FirstOrDefault(x => x.Id == orderId);

        if (order is null)
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
