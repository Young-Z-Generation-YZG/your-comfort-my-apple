

using YGZ.Basket.Domain.Core.Primitives;

namespace YGZ.Basket.Domain.ShoppingCart.ValueObjects;

public class CartLineId : ValueObject
{
    public Guid Value { get; private set; }

    public CartLineId(Guid value)
    {
        Value = value;
    }

    public static CartLineId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static CartLineId ToEntity(Guid value)
    {
        return new(value);
    }


    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
