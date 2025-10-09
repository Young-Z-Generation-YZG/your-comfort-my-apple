using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.Cache.Entities;

namespace YGZ.Basket.Infrastructure.Persistence.Repositories;

public class SKUPriceCacheRepository : ISKUPriceCache
{
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<SKUPriceCacheRepository> _logger;

    public SKUPriceCacheRepository(IDistributedCache distributedCache, ILogger<SKUPriceCacheRepository> logger)
    {
        _logger = logger;
        _distributedCache = distributedCache;
    }

    private static readonly DistributedCacheEntryOptions _cacheOptions = new();

    public async Task SetPriceAsync(PriceCache skuPrice, CancellationToken cancellationToken = default)
    {
        try
        {
            var cacheKey = skuPrice.GetCachedKey();
            var value = skuPrice.UnitPrice.ToString();

            await _distributedCache.SetStringAsync(cacheKey, value, _cacheOptions, cancellationToken);

            _logger.LogInformation("Price cached for model: {Model}, color: {Color}, storage: {Storage}, Price: {Price}",
                skuPrice.Model.Name, skuPrice.Color.Name, skuPrice.Storage.Name, value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error caching price for model: {Model}, color: {Color}, storage: {Storage}",
                skuPrice.Model.Name, skuPrice.Color.Name, skuPrice.Storage.Name);

            throw;
        }
    }

    public async Task SetPricesBatchAsync(IEnumerable<PriceCache> skuPrices, CancellationToken cancellationToken = default)
    {
        try
        {
            var tasks = skuPrices.Select(price => SetPriceAsync(price, cancellationToken));

            await Task.WhenAll(tasks);

            _logger.LogInformation("Batch cached {Count} SKU prices", skuPrices.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during batch price caching");

            throw;
        }
    }

    public async Task<decimal?> GetPriceAsync(PriceCache skuPrice, CancellationToken cancellationToken = default)
    {
        try
        {
            var cacheKey = skuPrice.GetCachedKey();
            var value = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);

            if (value is not null)
            {
                return decimal.Parse(value);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting price for model: {Model}, color: {Color}, storage: {Storage}",
                skuPrice.Model.Name, skuPrice.Color.Name, skuPrice.Storage.Name);

            throw;
        }

        return null;
    }

    public async Task<bool> ExistsAsync(PriceCache skuPrice, CancellationToken cancellationToken = default)
    {
        try
        {
            var cacheKey = skuPrice.GetCachedKey();
            var serializedData = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);

            return serializedData is not null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if price exists for model: {Model}, color: {Color}, storage: {Storage}",
                skuPrice.Model.Name, skuPrice.Color.Name, skuPrice.Storage.Name);

            throw;
        }
    }
}
