
using YGZ.Discount.Domain.Core.Primitives;

namespace YGZ.Discount.Domain.PromotionEvent.ValueObjects;

public class ProductId : ValueObject
{
    public string Value { get; set; }

    private ProductId(string value) => Value = value;

    public static ProductId Create(string value)
    {
        return new ProductId(value);
    }

    public static ProductId Of(string value)
    {
        return new ProductId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
