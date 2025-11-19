using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Core.Errors;
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
        var orderId = OrderId.Of(request.OrderId);

        var order = await _repository.GetByIdAsync(orderId, cancellationToken: cancellationToken);

        if (order is null)
        {
            return Errors.Order.DoesNotExist;
        }

        if (!EOrderStatus.TryFromName(request.UpdateStatus, true, out var newStatus))
        {
            return Errors.Order.InvalidOrderStatus;
        }

        switch (newStatus)
        {
            case var status when status == EOrderStatus.PREPARING:
            order.SetPreparing();
            break;
            case var status when status == EOrderStatus.DELIVERING:
            order.SetDelivering();
            break;
            case var status when status == EOrderStatus.DELIVERED:
            order.SetDelivered();
            break;
            default:
            return Errors.Order.InvalidOrderStatus;
        }

        order.UpdatedBy = _userContext.GetUserId();

        return await _repository.UpdateAsync(order, cancellationToken);
    }
}
