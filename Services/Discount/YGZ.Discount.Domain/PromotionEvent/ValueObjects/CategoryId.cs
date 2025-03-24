
using YGZ.Discount.Domain.Core.Primitives;

namespace YGZ.Discount.Domain.PromotionEvent.ValueObjects;

public class CategoryId : ValueObject
{
    public string Value { get; set; }

    private CategoryId(string value) => Value = value;

    public static CategoryId Create(string value)
    {
        return new CategoryId(value);
    }

    public static CategoryId Of(string value)
    {
        return new CategoryId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
