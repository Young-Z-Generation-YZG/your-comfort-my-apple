
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderHandler : ICommandHandler<DeleteOrderCommand, bool>
{
    private readonly IGenericRepository<Order, OrderId> _repository;
    private readonly IUserHttpContext _userContext;
    private readonly ILogger<DeleteOrderHandler> _logger;

    public DeleteOrderHandler(IGenericRepository<Order, OrderId> repository,
                              IUserHttpContext userContext,
                              ILogger<DeleteOrderHandler> logger)
    {
        _repository = repository;
        _userContext = userContext;
        _logger = logger;
    }

    public Task<Result<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
