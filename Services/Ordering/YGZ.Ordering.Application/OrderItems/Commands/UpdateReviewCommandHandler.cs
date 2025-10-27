

using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.OrderItems.Commands;

public class UpdateReviewCommandHandler : ICommandHandler<UpdateReviewCommand, BooleanRpcResponse>
{
    private readonly IGenericRepository<OrderItem, OrderItemId> _repository;
    private readonly ILogger<UpdateReviewCommandHandler> _logger;

    public UpdateReviewCommandHandler(IGenericRepository<OrderItem, OrderItemId> repository, ILogger<UpdateReviewCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<BooleanRpcResponse>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        //try
        //{
        //    if (!Guid.TryParse(request.OrderItemId, out var orderItemGuid))
        //    {
        //        return new BooleanRpcResponse
        //        {
        //            IsSuccess = false,
        //            ErrorMessage = "Invalid OrderItemId format."
        //        };
        //    }

        //    var id = OrderItemId.Of(orderItemGuid);

        //    var orderItem = await _repository.GetByIdAsync(id, cancellationToken);

        //    orderItem.CheckIsReviewed();

        //    var result = await _repository.UpdateAsync(orderItem, cancellationToken);

        //    return new BooleanRpcResponse
        //    {
        //        IsSuccess = result,
        //        ErrorMessage = null
        //    };
        //}
        //catch (Exception ex)
        //{
        //    _logger.LogError(ex, "Error updating review for OrderItemId: {OrderItemId}", request.OrderItemId);

        //    return new BooleanRpcResponse
        //    {
        //        IsSuccess = false,
        //        ErrorMessage = ex.Message
        //    };
        //}
    }
}
