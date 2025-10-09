using YGZ.Basket.Domain.Cache.Entities;

namespace YGZ.Basket.Application.Abstractions.Data;

public interface ISKUPriceCache
{
    Task<decimal?> GetPriceAsync(PriceCache skuPrice, CancellationToken cancellationToken = default);
    Task SetPriceAsync(PriceCache skuPrice, CancellationToken cancellationToken = default);
    Task SetPricesBatchAsync(IEnumerable<PriceCache> skuPrices, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(PriceCache skuPrice, CancellationToken cancellationToken = default);
}
