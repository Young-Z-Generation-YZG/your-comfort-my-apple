using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Application.Orders.Commands.CreateOrder.Extensions;

namespace YGZ.Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<CreateOrderCommandHandler> _logger;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, ILogger<CreateOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = request.ToEntity(request.OrderId, request.CustomerId, request.CustomerEmail);

        var result = await _orderRepository.AddAsync(order, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return true;
    }
}
