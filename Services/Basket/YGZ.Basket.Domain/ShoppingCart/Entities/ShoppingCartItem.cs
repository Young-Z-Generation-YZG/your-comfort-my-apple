

namespace YGZ.Basket.Domain.ShoppingCart.Entities;

public class ShoppingCartItem
{
    public string ProductId { get; set; } = default!;

    public string ProductModel { get; set; } = default!;

    public string ProductColor { get; set; } = default!;

    public string ProductColorHex { get; set; } = default!;

    public int ProductStorage { get; set; } = default!;

    public decimal ProductPrice { get; set; } = default!;

    public string ProductImage { get; set; } = default!;

    public int Quantity { get; set; } = default!;

    public static ShoppingCartItem Create(string ProductId,
                                          string ProductModel,
                                          string ProductColor,
                                          string ProductColorHex,
                                          int ProductStorage,
                                          decimal ProductPrice,
                                          string ProductImage,
                                          int Quantity)
    {
        return new ShoppingCartItem
        {
            ProductId = ProductId,
            ProductModel = ProductModel,
            ProductColor = ProductColor,
            ProductColorHex = ProductColorHex,
            ProductStorage = ProductStorage,
            ProductPrice = ProductPrice,
            ProductImage = ProductImage,
            Quantity = Quantity
        };
    }
}
