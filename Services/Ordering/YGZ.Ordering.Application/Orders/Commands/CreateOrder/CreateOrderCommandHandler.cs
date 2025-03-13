using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Ordering.Application.Abstractions;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Application.Orders.Commands.CreateOrder.Extensions;

namespace YGZ.Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<CreateOrderCommandHandler> _logger;
    private readonly IUserContext _userContext;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, ILogger<CreateOrderCommandHandler> logger, IUserContext userContext)
    {
        _orderRepository = orderRepository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        //var userId = _userContext.GetUserId();
        //var userEmail = _userContext.GetUserId();

        var order = request.ToEntity("d7610ca1-2909-49d3-af23-d502a297da29", "lov3rinve146@gmail.com");

        var result = await _orderRepository.AddAsync(order, cancellationToken);

        if(result.IsFailure)
        {
            return result.Error;
        }

        return true;
    }
}
