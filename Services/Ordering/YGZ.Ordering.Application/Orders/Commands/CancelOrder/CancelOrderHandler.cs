

using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.ValueObjects;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Core.Errors;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Commands.CancelOrder;

public class CancelOrderHandler : ICommandHandler<CancelOrderCommand, bool>
{
    private readonly ILogger<CancelOrderHandler> _logger;
    private readonly IGenericRepository<Order, OrderId> _repository;
    private readonly IUserHttpContext _userHttpContext;

    public CancelOrderHandler(IGenericRepository<Order, OrderId> repository,
                              IUserHttpContext userHttpContext,
                              ILogger<CancelOrderHandler> logger)
    {
        _logger = logger;
        _repository = repository;
        _userHttpContext = userHttpContext;
    }

    public async Task<Result<bool>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(request.OrderId);
        var userId = UserId.Of(_userHttpContext.GetUserId());

        var expressions = new Expression<Func<Order, object>>[]
        {
            x => x.OrderItems
        };

        var order = await _repository.GetByIdAsync(orderId, includes: expressions, cancellationToken: cancellationToken);

        if (order is null)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_repository.GetByIdAsync), "Order not found", new { orderId = request.OrderId, userId = userId.Value });

            return Errors.Order.DoesNotExist;
        }

        if (order.OrderStatus != EOrderStatus.PENDING)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Cannot cancel order - order is not in PENDING status", new { orderId = request.OrderId, currentStatus = order.OrderStatus.Name, userId = userId.Value });

            return Errors.Order.CannotCancelOrder;
        }

        order.SetCancelled();

        var updateResult = await _repository.UpdateAsync(order, cancellationToken);

        if (updateResult.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_repository.UpdateAsync), "Failed to cancel order", new { orderId = request.OrderId, userId = userId.Value, error = updateResult.Error });

            return updateResult.Error;
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully cancelled order", new { orderId = request.OrderId, userId = userId.Value });

        return updateResult;
    }
}
