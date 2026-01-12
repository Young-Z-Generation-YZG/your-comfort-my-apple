namespace YGZ.Catalog.Application.Abstractions.Caching;

/// <summary>
/// Service to manage cached product catalog data for chatbot context
/// </summary>
public interface IProductCatalogCacheService
{
    /// <summary>
    /// Gets the product catalog summary from cache for chatbot context
    /// </summary>
    Task<string?> GetProductCatalogSummaryAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes the product catalog cache by fetching latest data from database
    /// </summary>
    Task SetProductCatalogSummaryAsync(CancellationToken cancellationToken = default);
}
