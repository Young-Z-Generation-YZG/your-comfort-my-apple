

using YGZ.Basket.Domain.Core.Primitives;

namespace YGZ.Basket.Domain.ShoppingCart.ValueObjects;

public class ShoppingCartId : ValueObject
{
    public Guid Value { get; private set; }

    public ShoppingCartId(Guid value)
    {
        Value = value;
    }

    public static ShoppingCartId CreateUnique()
    {
        return new(Guid.NewGuid());
    }


    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
