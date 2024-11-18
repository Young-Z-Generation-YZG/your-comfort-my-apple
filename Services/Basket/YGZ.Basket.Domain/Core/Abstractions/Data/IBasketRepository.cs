




using YGZ.Basket.Domain.Core.Abstractions.Result;
using YGZ.Basket.Domain.ShoppingCart;

public interface IBasketRepository
{
    Task<Result<ShoppingCart>> GetBasket(string userId, CancellationToken cancellationToken = default);
    Task<Result<ShoppingCart>> StoreBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken);
    Task<Result<bool>> DeleteBasket(string userId, CancellationToken cancellationToken = default);
}
