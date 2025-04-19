
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.Catalog.Domain.Core.Abstractions;

namespace YGZ.Catalog.Infrastructure.Persistence.Interceptors;

public interface IDispatchDomainEventInterceptor
{
    Task BeforeInsert(IDomainEvent domainEvent);
}

public class DispatchDomainEventInterceptor : IDispatchDomainEventInterceptor
{
    private readonly IMediator _mediator;
    private readonly ILogger<DispatchDomainEventInterceptor> _logger;

    public DispatchDomainEventInterceptor(IMediator mediator, ILogger<DispatchDomainEventInterceptor> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task BeforeInsert(IDomainEvent domainEvent)
    {
        _logger.LogInformation("Dispatching domain event: {DomainEvent}", domainEvent.GetType().Name);

        await _mediator.Publish(domainEvent);
    }
}
