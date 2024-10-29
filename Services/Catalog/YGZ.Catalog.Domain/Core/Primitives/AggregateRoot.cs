
namespace YGZ.Catalog.Domain.Core.Primitives;

public abstract class AggregateRoot<TId> : Entity<TId> where TId : notnull
{
    protected AggregateRoot(TId id) : base(id)
    {
    }
}
