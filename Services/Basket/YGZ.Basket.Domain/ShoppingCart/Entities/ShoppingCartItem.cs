

using YGZ.Basket.Domain.ShoppingCart.ValueObjects;

namespace YGZ.Basket.Domain.ShoppingCart.Entities;

public class ShoppingCartItem
{
    public required bool IsSelected { get; init; }
    public required string ModelId { get; init; }
    public required string ProductName { get; init; }
    public required Model Model { get; init; }
    public required Color Color { get; init; }
    public required Storage Storage { get; init; }
    public required string DisplayImageUrl { get; init; }
    public required decimal UnitPrice { get; init; }
    public Promotion? Promotion { get; init; }
    public required int Quantity { get; init; }
    public required decimal SubTotalAmount { get; init; }
    public required string ModelSlug { get; init; }
    public required int Order { get; init; }

    public static ShoppingCartItem Create(
        bool isSelected,
        string modelId,
        string productName,
        Model model,
        Color color,
        Storage storage,
        string displayImageUrl,
        decimal unitPrice,
        Promotion? promotion,
        int quantity,
        decimal subTotalAmount,
        string modelSlug,
        int order
    )
    {
        return new ShoppingCartItem
        {
            IsSelected = isSelected,
            ModelId = modelId,
            ProductName = productName,
            Model = model,
            Color = color,
            Storage = storage,
            DisplayImageUrl = displayImageUrl,
            UnitPrice = unitPrice,
            Promotion = promotion,
            Quantity = quantity,
            SubTotalAmount = subTotalAmount,
            ModelSlug = modelSlug,
            Order = order
        };
    }
}
