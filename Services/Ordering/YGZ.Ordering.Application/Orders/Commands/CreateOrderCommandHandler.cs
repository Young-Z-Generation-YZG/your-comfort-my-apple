using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Ordering.Application.Orders.Commands;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, bool>
{
    public async Task<Result<bool>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        throw new NotImplementedException();
    }
}
