
namespace YGZ.Catalog.Domain.Core.Primitives;

public abstract class AggregateRoot<TId> : Entity<TId> where TId : ValueObject
{
    protected AggregateRoot(TId id) : base(id)
    {
    }
}