

namespace YGZ.Catalog.Application.Core.Abstractions.EventBus;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent message, CancellationToken cancellationToken = default) 
        where TEvent : class;
}
