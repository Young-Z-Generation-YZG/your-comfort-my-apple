
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Basket.Infrastructure.Persistence.Repositories;

public class CachedBasketRepository : IBasketRepository
{
    private readonly IBasketRepository _basketRepository;
    private readonly IDistributedCache _distributedCache;

    public CachedBasketRepository(IBasketRepository basketRepository, IDistributedCache distributedCache)
    {
        _basketRepository = basketRepository;
        _distributedCache = distributedCache;
    }

    public async Task<Result<ShoppingCart>> GetBasketAsync(string userEmail, CancellationToken cancellationToken)
    {
        var cachedBasket = await _distributedCache.GetStringAsync(userEmail, cancellationToken);

        if (!string.IsNullOrEmpty(cachedBasket))
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
        }

        var basket = await _basketRepository.GetBasketAsync(userEmail, cancellationToken);

        await _distributedCache.SetStringAsync(userEmail, JsonSerializer.Serialize(basket.Response), cancellationToken);

        return basket;
    }

    public async Task<Result<bool>> StoreBasketAsync(ShoppingCart shoppingCart, CancellationToken cancellationToken)
    {
        await _basketRepository.StoreBasketAsync(shoppingCart, cancellationToken);

        await _distributedCache.SetStringAsync(shoppingCart.UserEmail, JsonSerializer.Serialize(shoppingCart), cancellationToken);

        return true;
    }

    public async Task<Result<bool>> DeleteBasketAsync(string userEmail, CancellationToken cancellationToken)
    {
        await _basketRepository.DeleteBasketAsync(userEmail, cancellationToken);

        await _distributedCache.RemoveAsync(userEmail, cancellationToken);

        return true;
    }
}
