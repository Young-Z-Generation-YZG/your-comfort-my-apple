

using MediatR;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Abstractions.Data;

namespace YGZ.Catalog.Persistence.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly IMongoContext _context;
    private readonly IMediator _mediator;

    public UnitOfWork(IMongoContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<bool> CommitAsync(IEnumerable<IHasDomainEvents> domainEntities)

    {
        var changeAmount = await _context.SaveChanges();

        if(changeAmount > 0)
        {
            if(domainEntities is null) return true;

            var domainEvents = domainEntities.Where(e => e.DomainEvents.Any())
                                            .ToList()
                                            .SelectMany(e => e.DomainEvents)
                                            .ToList();

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }

            domainEntities.ToList().ForEach(e => e.ClearDomainEvents());
        }

        return changeAmount > 0;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}