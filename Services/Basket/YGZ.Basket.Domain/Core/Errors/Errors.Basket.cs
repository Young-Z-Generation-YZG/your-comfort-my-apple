using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Basket.Domain.Core.Errors;

public static partial class Errors
{
    public static class Basket
    {
        public static Error DoesNotExist = Error.BadRequest(code: "Basket.DoesNotExist", message: "Basket does not exists", serviceName: "BasketService");
        public static Error CannotStoreBasket = Error.BadRequest(code: "Basket.CannotStoreBasket", message: "Can not store basket", serviceName: "BasketService");
        public static Error CannotDeleteBasket = Error.BadRequest(code: "Basket.CannotDeleteBasket", message: "Can not delete basket", serviceName: "BasketService");
        public static Error BasketEmpty = Error.BadRequest(code: "Basket.BasketEmpty", message: "Basket empty", serviceName: "BasketService");
        public static Error NotSelectedItems = Error.BadRequest(code: "Basket.NotSelectedItems", message: "No items selected in the basket", serviceName: "BasketService");
        public static Error InvalidCryptoUUID = Error.BadRequest(code: "Basket.InvalidCryptoUUID", message: "Invalid crypto UUID", serviceName: "BasketService");
    }
}