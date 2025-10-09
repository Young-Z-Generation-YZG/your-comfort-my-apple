using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket;

public sealed record StoreBasketCommand(List<CartItemCommand> CartItems) : ICommand<bool> { }

public sealed record CartItemCommand
{
    public required string ModelId { get; init; }
    public required ModelCommand Model { get; init; }
    public required ColorCommand Color { get; init; }
    public required StorageCommand Storage { get; init; }
    public required int Quantity { get; init; }
}

public sealed record ModelCommand
{
    public required string Name { get; init; }
    public required string NormalizedName { get; init; }
}

public sealed record ColorCommand
{
    public required string Name { get; init; }
    public required string NormalizedName { get; init; }
}

public sealed record StorageCommand
{
    public required string Name { get; init; }
    public required string NormalizedName { get; init; }
}

//public sealed record PromotionCommand
//{
//    public required string PromotionType { get; init; }
//    public PromotionCouponCommand? PromotionCoupon { get; init; }
//    public PromotionEventCommand? PromotionEvent { get; init; }
//}

//public sealed record PromotionCouponCommand
//{
//    public required string CouponId { get; init; }
//}

//public sealed record PromotionEventCommand
//{
//    public required string EventId { get; init; }
//    public required string EventItemId { get; init; }
//}