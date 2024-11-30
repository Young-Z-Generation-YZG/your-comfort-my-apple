
using YGZ.Ordering.Application.Core.Abstractions.Messaging;
using YGZ.Ordering.Domain.Core.Abstractions.Result;

namespace YGZ.Ordering.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderCommand, bool>
{
    public async Task<Result<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
