

using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Domain.PromotionEvent.ValueObjects;

public class PromotionEventId : ValueObject
{
    public Guid? Value { get; set; } = null;

    private PromotionEventId() { }
    private PromotionEventId(Guid? guid) => Value = guid;

    public static PromotionEventId Create()
    {
        return new PromotionEventId(Guid.NewGuid());
    }

    public static PromotionEventId Of(Guid? guid)
    {
        return new PromotionEventId(guid);
    }

    public static PromotionEventId Of(string guid)
    {
        var isParsed = Guid.TryParse(guid, out Guid result);

        return new PromotionEventId(result);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
