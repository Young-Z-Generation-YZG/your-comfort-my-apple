

using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.CatalogServices;
using YGZ.Ordering.Application.OrderItems.Commands.UpdateIsReviewed;

namespace YGZ.Ordering.Application.Orders.Events.Integrations;

public class ReviewCreatedIntegrationEventHandler : IConsumer<ReviewCreatedIntegrationEvent>
{
    private readonly ISender _sender;

    private readonly ILogger<ReviewCreatedIntegrationEvent> _logger;

    public ReviewCreatedIntegrationEventHandler(ISender sender, ILogger<ReviewCreatedIntegrationEvent> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ReviewCreatedIntegrationEvent> context)
    {
        _logger.LogInformation("Integration event handled: {IntegrationEvent}", context.Message.GetType().Name);

        UpdateIsReviewedCommand command = new UpdateIsReviewedCommand
        {
            OrderItemId = context.Message.OrderItemId
        };

        await _sender.Send(command);
    }
}
