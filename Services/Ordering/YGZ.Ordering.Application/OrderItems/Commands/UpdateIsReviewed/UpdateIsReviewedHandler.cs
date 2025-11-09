using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.OrderItems.Commands.UpdateIsReviewed;

public class UpdateIsReviewedHandler : ICommandHandler<UpdateIsReviewedCommand, bool>
{
    private readonly ILogger<UpdateIsReviewedHandler> _logger;
    private readonly IGenericRepository<OrderItem, OrderItemId> _repository;

    public UpdateIsReviewedHandler(ILogger<UpdateIsReviewedHandler> logger,
                                   IGenericRepository<OrderItem, OrderItemId> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(UpdateIsReviewedCommand request, CancellationToken cancellationToken)
    {
        var guidParsed = Guid.TryParse(request.OrderItemId, out var orderItemGuid);

        if (!guidParsed)
        {
            return false;
        }

        var orderItem = await _repository.GetByIdAsync(OrderItemId.Of(orderItemGuid), null, cancellationToken);

        if (orderItem is null)
        {
            return false;
        }

        orderItem.SetIsReviewed(request.IsReviewed);

        var result = await _repository.UpdateAsync(orderItem, cancellationToken);

        return result.IsSuccess;
    }
}
