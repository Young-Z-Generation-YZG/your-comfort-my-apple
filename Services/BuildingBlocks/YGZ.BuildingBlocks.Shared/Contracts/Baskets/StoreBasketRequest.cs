

namespace YGZ.BuildingBlocks.Shared.Contracts.Baskets;

public record StoreBasketRequest(string UserId, List<CartLineRequest> Cart_lines) { }

