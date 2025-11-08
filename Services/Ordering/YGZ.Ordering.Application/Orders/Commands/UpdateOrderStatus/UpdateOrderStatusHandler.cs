

using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusHandler : ICommandHandler<UpdateOrderStatusCommand, bool>
{
    private readonly IGenericRepository<Order, OrderId> _repository;
    private readonly IUserHttpContext _userContext;
    private readonly ILogger<UpdateOrderStatusHandler> _logger;

    public UpdateOrderStatusHandler(IGenericRepository<Order, OrderId> repository,
                                    IUserHttpContext userContext,
                                    ILogger<UpdateOrderStatusHandler> logger)
    {
        _repository = repository;
        _userContext = userContext;
        _logger = logger;
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
