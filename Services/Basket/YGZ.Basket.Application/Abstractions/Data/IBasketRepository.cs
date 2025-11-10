

using YGZ.Basket.Domain.ShoppingCart;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Basket.Application.Abstractions.Data;

public interface IBasketRepository
{
    Task<Result<ShoppingCart>> GetBasketAsync(string userEmail, CancellationToken cancellationToken);
    Task<Result<bool>> StoreBasketAsync(ShoppingCart shoppingCart, CancellationToken cancellationToken);
    Task<Result<bool>> DeleteSelectedItemsBasketAsync(string userEmail, CancellationToken cancellationToken);
    Task<Result<bool>> ClearBasketAsync(string userEmail, CancellationToken cancellationToken);
}
