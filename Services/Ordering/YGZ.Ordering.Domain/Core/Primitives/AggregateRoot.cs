


namespace YGZ.Ordering.Domain.Core.Primitives;

public abstract class AggregateRoot<TId> : Entity<TId> where TId : ValueObject
{
    protected AggregateRoot(TId id) : base(id)
    {
    }
}