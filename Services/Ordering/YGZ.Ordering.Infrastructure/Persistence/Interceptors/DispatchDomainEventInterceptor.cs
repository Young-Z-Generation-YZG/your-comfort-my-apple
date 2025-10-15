
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using YGZ.BuildingBlocks.Shared.Domain.Core.Abstractions;

namespace YGZ.Ordering.Infrastructure.Persistence.Interceptors;

public class DispatchDomainEventInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;

    public DispatchDomainEventInterceptor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEvents(DbContext? context)
    {
        if (context == null)
        {
            return;
        }

        var domainEventEntities = context.ChangeTracker
                                    .Entries<IAggregate>()
                                    .Where(a => a.Entity.DomainEvents.Any())
                                    .Select(a => a.Entity);

        var domainEvents = domainEventEntities
                            .SelectMany(a => a.DomainEvents)
                            .ToList();

        foreach (var entity in domainEventEntities)
        {
            entity.ClearDomainEvents();
        }

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }
    }
}
