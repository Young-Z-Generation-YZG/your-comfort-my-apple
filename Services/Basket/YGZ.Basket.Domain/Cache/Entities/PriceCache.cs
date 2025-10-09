using YGZ.Basket.Domain.ShoppingCart.ValueObjects;

namespace YGZ.Basket.Domain.Cache.Entities;

public class PriceCache
{
    public required Model Model { get; init; }
    public required Color Color { get; init; }
    public required Storage Storage { get; init; }
    public required decimal UnitPrice { get; init; }

    public static PriceCache Create(Model model,
                                  Color color,
                                  Storage storage,
                                  decimal unitPrice)
    {
        return new PriceCache
        {
            Model = model,
            Color = color,
            Storage = storage,
            UnitPrice = unitPrice
        };
    }

    public static PriceCache Of(Model model,
                              Color color,
                              Storage storage)
    {
        return new PriceCache
        {
            Model = model,
            Color = color,
            Storage = storage,
            UnitPrice = 0M
        };
    }

    public string GetCachedKey() => $"{Model.NormalizedName}:{Color.NormalizedName}:{Storage.NormalizedName}";
}
