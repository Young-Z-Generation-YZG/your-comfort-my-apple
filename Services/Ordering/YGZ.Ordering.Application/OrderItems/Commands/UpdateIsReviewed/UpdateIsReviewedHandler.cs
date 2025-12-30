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
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Invalid order item ID format", new { orderItemId = request.OrderItemId, isReviewed = request.IsReviewed });

            return false;
        }

        var orderItem = await _repository.GetByIdAsync(OrderItemId.Of(orderItemGuid), null, cancellationToken);

        if (orderItem is null)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_repository.GetByIdAsync), "Order item not found", new { orderItemId = request.OrderItemId, isReviewed = request.IsReviewed });

            return false;
        }

        orderItem.SetIsReviewed(request.IsReviewed);

        var result = await _repository.UpdateAsync(orderItem, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_repository.UpdateAsync), "Failed to update order item review status", new { orderItemId = request.OrderItemId, isReviewed = request.IsReviewed, error = result.Error });

            return false;
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully updated order item review status", new { orderItemId = request.OrderItemId, isReviewed = request.IsReviewed });

        return result.IsSuccess;
    }
}
