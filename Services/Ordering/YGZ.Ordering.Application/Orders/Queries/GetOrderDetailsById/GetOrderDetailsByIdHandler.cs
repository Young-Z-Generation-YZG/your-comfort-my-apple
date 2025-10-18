
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.Ordering.Application.Abstractions;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Core.Errors;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Queries.GetOrderItemsByOrderId;

public class GetOrderDetailsByIdHandler : IQueryHandler<GetOrderDetailsByIdQuery, OrderDetailsResponse>
{
    private readonly IGenericRepository<Order, OrderId> _repository;
    private readonly IUserRequestContext _userContext;
    private readonly ILogger<GetOrderDetailsByIdHandler> _logger;

    public GetOrderDetailsByIdHandler(IUserRequestContext userContext,
                                      IGenericRepository<Order, OrderId> repository,
                                      ILogger<GetOrderDetailsByIdHandler> logger)
    {
        _repository = repository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result<OrderDetailsResponse>> Handle(GetOrderDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        OrderId orderId = OrderId.Of(request.OrderId);

        var orderResult = await _repository.GetByIdAsync(orderId, cancellationToken, x => x.OrderItems);

        if (orderResult is null)
        {
            return Errors.Order.DoesNotExist;
        }

        return orderResult.ToOrderDetailsResponse();
    }
}
