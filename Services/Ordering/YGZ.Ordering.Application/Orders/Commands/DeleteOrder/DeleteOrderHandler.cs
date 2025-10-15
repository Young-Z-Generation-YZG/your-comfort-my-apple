
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Ordering.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderHandler : ICommandHandler<DeleteOrderCommand, bool>
{
    public Task<Result<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
