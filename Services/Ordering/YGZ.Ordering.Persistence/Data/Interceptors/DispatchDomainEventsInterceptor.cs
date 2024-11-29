
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using YGZ.Ordering.Domain.Core.Abstractions;

namespace YGZ.Ordering.Persistence.Data.Interceptors;

public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;

    public DispatchDomainEventsInterceptor(IMediator mediator)
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

    public async Task DispatchDomainEvents(DbContext? dbContext)
    {
        if (dbContext == null) return;

        var aggregates = dbContext.ChangeTracker.Entries<IHasDomainEvents>()
                                                .Where(a => a.Entity.DomainEvents.Any())
                                                .Select(a => a.Entity);

        var domainEvents = aggregates.SelectMany(a => a.DomainEvents).ToList();

        foreach (var domainEvent in domainEvents) { 
            await _mediator.Publish(domainEvent);
        }

        aggregates.ToList().ForEach(x => x.ClearDomainEvents());
    }
}
