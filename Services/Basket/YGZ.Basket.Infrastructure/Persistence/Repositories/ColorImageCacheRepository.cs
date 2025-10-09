using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.Cache.Entities;

namespace YGZ.Basket.Infrastructure.Persistence.Repositories;

public class ColorImageCacheRepository : IColorImageCache
{
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<ColorImageCacheRepository> _logger;

    public ColorImageCacheRepository(IDistributedCache distributedCache, ILogger<ColorImageCacheRepository> logger)
    {
        _logger = logger;
        _distributedCache = distributedCache;
    }

    private static readonly DistributedCacheEntryOptions _cacheOptions = new();

    public async Task SetImageUrlAsync(ColorImageCache colorImage, CancellationToken cancellationToken = default)
    {
        try
        {
            var cacheKey = colorImage.GetCachedKey();
            var value = colorImage.DisplayImageUrl;

            await _distributedCache.SetStringAsync(cacheKey, value, _cacheOptions, cancellationToken);

            _logger.LogInformation("Color image cached for modelId: {ModelId}, color: {Color}, ImageUrl: {ImageUrl}",
                colorImage.ModelId, colorImage.Color.Name, value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error caching color image for modelId: {ModelId}, color: {Color}",
                colorImage.ModelId, colorImage.Color.Name);

            throw;
        }
    }

    public async Task SetImageUrlsBatchAsync(IEnumerable<ColorImageCache> colorImages, CancellationToken cancellationToken = default)
    {
        try
        {
            var tasks = colorImages.Select(image => SetImageUrlAsync(image, cancellationToken));

            await Task.WhenAll(tasks);

            _logger.LogInformation("Batch cached {Count} color images", colorImages.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during batch color image caching");

            throw;
        }
    }

    public async Task<string?> GetImageUrlAsync(ColorImageCache colorImage, CancellationToken cancellationToken = default)
    {
        try
        {
            var cacheKey = colorImage.GetCachedKey();
            var value = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);

            return value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting color image for modelId: {ModelId}, color: {Color}",
                colorImage.ModelId, colorImage.Color.Name);

            throw;
        }
    }

    public async Task<bool> ExistsAsync(ColorImageCache colorImage, CancellationToken cancellationToken = default)
    {
        try
        {
            var cacheKey = colorImage.GetCachedKey();
            var value = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);

            return value is not null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if color image exists for modelId: {ModelId}, color: {Color}",
                colorImage.ModelId, colorImage.Color.Name);

            throw;
        }
    }
}
