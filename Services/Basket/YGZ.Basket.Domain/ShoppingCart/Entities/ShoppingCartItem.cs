

using YGZ.Basket.Domain.ShoppingCart.ValueObjects;

namespace YGZ.Basket.Domain.ShoppingCart.Entities;

public class ShoppingCartItem
{
    required public string ProductId { get; set; }
    required public string ProductName { get; set; }
    required public string ProductColorName { get; set; }
    required public decimal ProductUnitPrice { get; set; }
    required public string ProductNameTag { get; set; }
    required public string ProductImage { get; set; }
    required public string ProductSlug { get; set; }
    required public string CategoryId { get; set; }
    public int Quantity { get; set; } = 0;
    public Promotion? Promotion { get; set; } = null;
    public decimal? SubTotalAmount { get; set; } = 0;
    required public int OrderIndex { get; set; }

    public static ShoppingCartItem Create(string productId,
                                          string productName,
                                          string productColorName,
                                          decimal productUnitPrice,
                                          string productNameTag,
                                          string productImage,
                                          string productSlug,
                                          string categoryId,
                                          int quantity,
                                          Promotion? promotion,
                                          decimal? subTotalAmount,
                                          int orderIndex)
    {
        return new ShoppingCartItem
        {
            ProductId = productId,
            ProductName = productName,
            ProductColorName = productColorName,
            ProductUnitPrice = productUnitPrice,
            ProductNameTag = productNameTag,
            ProductImage = productImage,
            ProductSlug = productSlug,
            CategoryId = categoryId,
            Quantity = quantity,
            Promotion = promotion,
            SubTotalAmount = subTotalAmount,
            OrderIndex = orderIndex
        };
    }
}
