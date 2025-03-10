using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket;

#pragma warning disable CS8618

public sealed record StoreBasketCommand(List<CartItemCommand> Cart) : ICommand<bool> { }


public sealed record CartItemCommand
{
    public string ProductId { get; set; }

    public string ProductModel { get; set; }

    public string ProductColor { get; set; }

    public string ProductColorHex { get; set; }

    public int ProductStorage { get; set; }

    public decimal ProductPrice { get; set; }

    public string ProductImage { get; set; }

    public int Quantity { get; set; }
}