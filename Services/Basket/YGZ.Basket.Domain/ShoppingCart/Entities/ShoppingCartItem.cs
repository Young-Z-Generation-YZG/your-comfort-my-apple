

namespace YGZ.Basket.Domain.ShoppingCart.Entities;

public class ShoppingCartItem
{
    public string ProductId { get; set; } = default!;

    public string ProductModel { get; set; } = default!;

    public string ProductColor { get; set; } = default!;

    public int ProductStorage { get; set; } = default!;

    public decimal ProductUnitPrice { get; set; } = default!;

    public string ProductNameTag { get; set; }

    public string ProductImage { get; set; } = default!;

    public int Quantity { get; set; } = default!;

    public static ShoppingCartItem Create(string productId,
                                          string productModel,
                                          string productColor,
                                          int productStorage,
                                          decimal productUnitPrice,
                                          string productNameTag,
                                          string productImage,
                                          int quantity)
    {
        return new ShoppingCartItem
        {
            ProductId = productId,
            ProductModel = productModel,
            ProductColor = productColor,
            ProductStorage = productStorage,
            ProductUnitPrice = productUnitPrice,
            ProductNameTag = productNameTag,
            ProductImage = productImage,
            Quantity = quantity
        };
    }
}
