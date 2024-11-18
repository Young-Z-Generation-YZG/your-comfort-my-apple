

using YGZ.Basket.Domain.Core.Primitives;

namespace YGZ.Basket.Domain.ShoppingCart.ValueObjects;

public class UserId : ValueObject
{
    public Guid Value { get; private set; }

    public UserId(Guid value)
    {
        Value = value;
    }

    public static ShoppingCartId CreateUnique()
    {
        return new(new Guid());
    }


    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
