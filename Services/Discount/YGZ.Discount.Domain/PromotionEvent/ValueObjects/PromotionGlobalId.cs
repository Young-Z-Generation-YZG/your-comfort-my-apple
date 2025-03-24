

using YGZ.Discount.Domain.Core.Primitives;

namespace YGZ.Discount.Domain.PromotionEvent.ValueObjects;

public class PromotionGlobalId : ValueObject
{
    public Guid? Value { get; set; } = null;

    private PromotionGlobalId(Guid? guid) => Value = guid;

    public static PromotionGlobalId Create()
    {
        return new PromotionGlobalId(Guid.NewGuid());
    }

    public static PromotionGlobalId Of(Guid? guid)
    {
        return new PromotionGlobalId(guid);
    }

    public static PromotionGlobalId Of(string guid)
    {
        Guid.TryParse(guid, out Guid result);

        return new PromotionGlobalId(result);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
