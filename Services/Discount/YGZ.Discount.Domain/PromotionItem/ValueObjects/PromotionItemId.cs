
using YGZ.Discount.Domain.Core.Primitives;

namespace YGZ.Discount.Domain.PromotionItem.ValueObjects;

public class PromotionItemId : ValueObject
{
    public Guid? Value { get; set; } = null;

    private PromotionItemId(Guid? guid) => Value = guid;

    public static PromotionItemId Create()
    {
        return new PromotionItemId(Guid.NewGuid());
    }

    public static PromotionItemId Of(Guid? guid)
    {
        return new PromotionItemId(guid);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
