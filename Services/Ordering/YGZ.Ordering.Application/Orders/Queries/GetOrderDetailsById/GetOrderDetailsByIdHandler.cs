
using System.Linq.Expressions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Notifications;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.BuildingBlocks.Shared.Notifications.Hubs;
using YGZ.BuildingBlocks.Shared.Notifications.Models;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Core.Errors;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Queries.GetOrderItemsByOrderId;

public class GetOrderDetailsByIdHandler : IQueryHandler<GetOrderDetailsByIdQuery, OrderDetailsResponse>
{
    private readonly ILogger<GetOrderDetailsByIdHandler> _logger;
    private readonly IGenericRepository<Order, OrderId> _repository;
    private readonly IUserHttpContext _userContext;
    private readonly IHubContext<OrderNotificationHub, IOrderNotificationClient> _hubContext;

    public GetOrderDetailsByIdHandler(IUserHttpContext userContext,
                                      IGenericRepository<Order, OrderId> repository,
                                      ILogger<GetOrderDetailsByIdHandler> logger,
                                      IHubContext<OrderNotificationHub, IOrderNotificationClient> hubContext)
    {
        _repository = repository;
        _userContext = userContext;
        _logger = logger;
        _hubContext = hubContext;
    }

    public async Task<Result<OrderDetailsResponse>> Handle(GetOrderDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        OrderId orderId = OrderId.Of(request.OrderId);

        var expressions = new Expression<Func<Order, object>>[]
        {
            x => x.OrderItems
        };

        var result = await _repository.GetByIdAsync(orderId, expressions, cancellationToken);

        if (result is null)
        {
            return Errors.Order.DoesNotExist;
        }

        var orderNotificationModel = new OrderNotificationModel
        {
            OrderId = result.Id.Value,
            UserId = _userContext.GetUserId(),
            Status = result.OrderStatus.Name
        };

        await _hubContext.Clients.User(orderNotificationModel.UserId).OrderStatusUpdated(orderNotificationModel);

        return result.ToResponse();
    }
}
