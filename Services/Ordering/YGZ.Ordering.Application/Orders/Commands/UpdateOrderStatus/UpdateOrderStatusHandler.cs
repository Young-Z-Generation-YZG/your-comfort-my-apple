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
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_repository.GetByIdAsync), "Order not found", new { orderId = request.OrderId, updateStatus = request.UpdateStatus, userId = _userContext.GetUserId() });

            return Errors.Order.DoesNotExist;
        }

        if (!EOrderStatus.TryFromName(request.UpdateStatus, true, out var newStatus))
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Invalid order status", new { orderId = request.OrderId, updateStatus = request.UpdateStatus, userId = _userContext.GetUserId() });

            return Errors.Order.InvalidOrderStatus;
        }

        switch (newStatus.Name)
        {
            case nameof(EOrderStatus.PREPARING):
            order.SetPreparing();
            break;
            case nameof(EOrderStatus.DELIVERING):
            order.SetDelivering();
            break;
            case nameof(EOrderStatus.DELIVERED):
            order.SetDelivered();
            break;
            default:
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Invalid order status transition", new { orderId = request.OrderId, updateStatus = request.UpdateStatus, currentStatus = order.OrderStatus.Name, userId = _userContext.GetUserId() });

            return Errors.Order.InvalidOrderStatus;
        }

        order.UpdatedBy = _userContext.GetUserId();
        order.UpdatedAt = DateTime.UtcNow;

        var updateResult = await _repository.UpdateAsync(order, cancellationToken);

        if (updateResult.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_repository.UpdateAsync), "Failed to update order status", new { orderId = request.OrderId, updateStatus = request.UpdateStatus, userId = _userContext.GetUserId(), error = updateResult.Error });

            return updateResult.Error;
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully updated order status", new { orderId = request.OrderId, oldStatus = order.OrderStatus.Name, newStatus = newStatus.Name, userId = _userContext.GetUserId() });

        return updateResult;

    }
}