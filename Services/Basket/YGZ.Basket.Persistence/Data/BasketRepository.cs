

using Marten;
using YGZ.Basket.Domain.Core.Abstractions.Result;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.Basket.Domain.ShoppingCart;

namespace YGZ.Basket.Persistence.Data;

public class BasketRepository : IBasketRepository
{
    private readonly IDocumentSession _sesssion;

    public BasketRepository(IDocumentSession sesssion)
    {
        _sesssion = sesssion;
    }

    async Task<Result<ShoppingCart>> IBasketRepository.GetBasket(string UserId, CancellationToken cancellationToken)
    {
        var basket = await _sesssion.LoadAsync<ShoppingCart>(UserId, cancellationToken);

        if(basket is null)
        {
            return Errors.Basket.DoesNotExist;
        }

        return basket;
    }

    public async Task<Result<bool>> DeleteBasket(string userId, CancellationToken cancellationToken = default)
    {
        _sesssion.Delete<ShoppingCart>(userId);
        await _sesssion.SaveChangesAsync(cancellationToken);

        return true;
    }


    public async Task<Result<ShoppingCart>> StoreBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken)
    {
        _sesssion.Store(shoppingCart);
        await _sesssion.SaveChangesAsync(cancellationToken);

        return shoppingCart;
    }
}
