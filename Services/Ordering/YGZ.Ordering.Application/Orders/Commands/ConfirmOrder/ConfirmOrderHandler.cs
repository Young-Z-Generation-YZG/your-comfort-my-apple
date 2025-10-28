

using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.ValueObjects;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Orders.ValueObjects;
using YGZ.Ordering.Domain.Core.Errors;

namespace YGZ.Ordering.Application.Orders.Commands.ConfirmOrder;

public class ConfirmOrderHandler : ICommandHandler<ConfirmOrderCommand, bool>
{
    private readonly ILogger<ConfirmOrderHandler> _logger;
    private readonly IGenericRepository<Order, OrderId> _repository;
    private readonly IUserHttpContext _userHttpContext;

    public ConfirmOrderHandler(IGenericRepository<Order, OrderId> repository,
                              IUserHttpContext userHttpContext,
                              ILogger<ConfirmOrderHandler> logger)
    {
        _logger = logger;
        _repository = repository;
        _userHttpContext = userHttpContext;
    }

    public async Task<Result<bool>> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
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
            return Errors.Order.DoesNotExist;
        }

        if (order.OrderStatus != EOrderStatus.PENDING)
        {
           return Errors.Order.CannotConfirmOrder;
        }

        order.SetConfirmed();

        return await _repository.UpdateAsync(order, cancellationToken);
    }
}
