using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.Catalog.Domain.Tenants.Events;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Catalog.Application.Inventory.Events.DomainEvents;

public class ReservedQuantityDeductedDomainEventHandler : INotificationHandler<ReservedQuantityDeductedDomainEvent>
{
    private readonly ILogger<ReservedQuantityDeductedDomainEventHandler> _logger;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    public ReservedQuantityDeductedDomainEventHandler(ILogger<ReservedQuantityDeductedDomainEventHandler> logger,
                                                      DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _logger = logger;
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task Handle(ReservedQuantityDeductedDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.Sku.ReservedForEvent is null)
        {
            return;
        }

        await _discountProtoServiceClient.DeductEventItemQuantityGpcAsync(new DeductEventItemQuantityGpcRequest
        {
            EventItemId = notification.Sku.ReservedForEvent.EventItemId,
            EventId = notification.Sku.ReservedForEvent.EventId,
            DeductQuantity = 1
        });
    }
}
