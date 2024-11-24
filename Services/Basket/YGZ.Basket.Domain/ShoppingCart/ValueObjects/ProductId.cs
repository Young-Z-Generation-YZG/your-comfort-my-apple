

using YGZ.Basket.Domain.Core.Primitives;

namespace YGZ.Basket.Domain.ShoppingCart.ValueObjects;

public class ProductId : ValueObject
{
    public string Value { get; private set; }

    public ProductId(string value)
    {
        Value = value;
    }

    //public static ShoppingCartId CreateUnique()
    //{
    //    return new(new Guid());
    //}

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
