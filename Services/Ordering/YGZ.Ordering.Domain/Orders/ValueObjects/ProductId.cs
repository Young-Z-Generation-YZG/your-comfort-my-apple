

using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class ProductId : ValueObject
{
    public string Value { get; set; } = default!;

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public ProductId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Product id cannot be empty", nameof(value));
        }

        Value = value;
    }

    public static ProductId CreateNew()
    {
        return new ProductId(Guid.NewGuid().ToString());
    }

    public static ProductId Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Product id cannot be empty", nameof(value));
        }

        return new ProductId(value);
    }
}
