

using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using YGZ.Basket.Domain.Core.Abstractions.Result;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Persistence.Helpers;

namespace YGZ.Basket.Persistence.Data;

public class CachedBasketRepository : IBasketRepository
{
    private readonly IBasketRepository _basketRepository;
    private readonly IDistributedCache _cache;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public CachedBasketRepository(IBasketRepository basketRepository, IDistributedCache cache, JsonSerializerOptions jsonSerializerOptions)
    {
        _basketRepository = basketRepository;
        _cache = cache;
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public async Task<Result<bool>> DeleteBasket(string userId, CancellationToken cancellationToken = default)
    {
        await _basketRepository.DeleteBasket(userId, cancellationToken);

        await _cache.RemoveAsync(userId, cancellationToken);

        return true;
    }

    public async Task<Result<ShoppingCart>> GetBasket(string userId, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await _cache.GetStringAsync(userId, cancellationToken);

        if (!string.IsNullOrEmpty(cachedBasket)) {
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket, _jsonSerializerOptions)!;
        }

        var basket = await _basketRepository.GetBasket(userId, cancellationToken);

        await _cache.SetStringAsync(userId, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<Result<ShoppingCart>> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken)
    {
        await _basketRepository.StoreBasket(basket, cancellationToken);

        await _cache.SetStringAsync(basket.UserIdValue, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }
}
