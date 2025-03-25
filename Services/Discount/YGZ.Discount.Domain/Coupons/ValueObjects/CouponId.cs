

using YGZ.Discount.Domain.Core.Primitives;

namespace YGZ.Discount.Domain.Coupons.ValueObjects;

public class CouponId : ValueObject
{
    public Guid? Value { get; set; } = null;

    private CouponId(Guid? guid) => Value = guid;

    public static CouponId Create()
    {
        return new CouponId(Guid.NewGuid());
    }

    public static CouponId Of(Guid? guid)
    {
        return new CouponId(guid);
    }

    public static CouponId Of(string guid)
    {
        return new CouponId(Guid.Parse(guid));
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
