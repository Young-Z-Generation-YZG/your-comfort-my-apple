

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
        return new(new Guid());
    }


    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
