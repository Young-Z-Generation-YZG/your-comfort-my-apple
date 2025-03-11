
using System;
using YGZ.Discount.Domain.Core.Primitives;

namespace YGZ.Discount.Domain.Coupons.ValueObjects;

public class CouponId : ValueObject, IEquatable<CouponId>
{
    public Guid Id { get; private set; }

    public static CouponId Create(Guid value)
    {
        return new CouponId { Id = value };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }

    public bool Equals(CouponId? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as CouponId);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static DateTime CreatedTime => DateTime.UtcNow;
}
