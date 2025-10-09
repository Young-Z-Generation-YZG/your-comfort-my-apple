using YGZ.Basket.Domain.Cache.Entities;

namespace YGZ.Basket.Application.Abstractions.Data;

public interface IColorImageCache
{
    Task<string?> GetImageUrlAsync(ColorImageCache colorImage, CancellationToken cancellationToken = default);
    Task SetImageUrlAsync(ColorImageCache colorImage, CancellationToken cancellationToken = default);
    Task SetImageUrlsBatchAsync(IEnumerable<ColorImageCache> colorImages, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(ColorImageCache colorImage, CancellationToken cancellationToken = default);
}
