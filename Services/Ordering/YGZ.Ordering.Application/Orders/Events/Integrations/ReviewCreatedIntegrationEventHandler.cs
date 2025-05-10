

using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.CatalogServices;
using YGZ.Ordering.Application.OrderItems.Commands;
using YGZ.Ordering.Application.Orders.Events.Extensions;

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
        _logger.LogInformation("Integration envent handled: {IntegrationEvent}", context.Message.GetType().Name);

        UpdateReviewCommand command = context.ToCommand();

        await _sender.Send(command);
    }
}
