

using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Ordering.Application.Abstractions;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Commands.ConfirmOrder;

public class ConfirmOrderHandler : ICommandHandler<ConfirmOrderCommand, bool>
{
    private readonly IGenericRepository<Order, OrderId> _repository;
    private readonly IUserRequestContext _userContext;
    private readonly ILogger<ConfirmOrderHandler> _logger;

    public ConfirmOrderHandler(IGenericRepository<Order, OrderId> repository,
                              IUserRequestContext userContext,
                              ILogger<ConfirmOrderHandler> logger)
    {
        _repository = repository;
        _userContext = userContext;
        _logger = logger;
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
