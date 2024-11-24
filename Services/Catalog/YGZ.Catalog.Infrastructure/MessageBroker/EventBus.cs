

using MassTransit;
using YGZ.Catalog.Application.Core.Abstractions.EventBus;

namespace YGZ.Catalog.Infrastructure.MessageBroker;

internal sealed class EventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync<TEvent>(TEvent message, CancellationToken cancellationToken = default) where TEvent : class 
        => _publishEndpoint.Publish(message, cancellationToken);
}
