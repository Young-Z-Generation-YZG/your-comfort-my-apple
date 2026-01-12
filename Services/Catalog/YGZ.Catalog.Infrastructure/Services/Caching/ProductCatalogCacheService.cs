using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using YGZ.Catalog.Application.Abstractions.Caching;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.ProductModels;
using YGZ.Catalog.Domain.Products.ProductModels.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Services.Caching;

public class ProductCatalogCacheService : IProductCatalogCacheService
{
    private const string CacheKey = "chatbot:product_catalog_summary";
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromHours(24);

    private readonly IDistributedCache _cache;
    private readonly IMongoRepository<ProductModel, ModelId> _productModelRepository;
    private readonly ILogger<ProductCatalogCacheService> _logger;

    public ProductCatalogCacheService(
        IDistributedCache cache,
        IMongoRepository<ProductModel, ModelId> productModelRepository,
        ILogger<ProductCatalogCacheService> logger)
    {
        _cache = cache;
        _productModelRepository = productModelRepository;
        _logger = logger;
    }

    public async Task<string?> GetProductCatalogSummaryAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var cachedData = await _cache.GetStringAsync(CacheKey, cancellationToken);

            if (string.IsNullOrEmpty(cachedData))
            {
                _logger.LogInformation(":::[Service:{ServiceName}]::: Cache miss, refreshing product catalog cache",
                    nameof(ProductCatalogCacheService));

                await SetProductCatalogSummaryAsync(cancellationToken);
                cachedData = await _cache.GetStringAsync(CacheKey, cancellationToken);
            }

            return cachedData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ":::[Service:{ServiceName}][Exception:{ExceptionType}]::: Error getting product catalog from cache",
                nameof(ProductCatalogCacheService), ex.GetType().Name);
            return null;
        }
    }

    public async Task SetProductCatalogSummaryAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(":::[Service:{ServiceName}]::: Refreshing product catalog cache",
                nameof(ProductCatalogCacheService));

            var (products, _, _) = await _productModelRepository.GetAllAsync(
                _page: 1,
                _limit: 100,
                filter: null,
                sort: null,
                cancellationToken: cancellationToken);

            var productSummaries = products.Select(p => new ProductSummary
            {
                Name = p.Name,
                Category = p.Category?.Name ?? "Unknown",
                Models = p.Models.Select(m => m.Name).ToList(),
                Colors = p.Colors.Select(c => c.Name).ToList(),
                Storages = p.Storages.Select(s => s.Name).ToList(),
                PriceRange = GetPriceRange(p.Prices),
                Description = TruncateDescription(p.Description, 200),
                AverageRating = (double)(p.AverageRating?.RatingAverageValue ?? 0),
                OverallSold = p.OverallSold,
                IsNewest = p.IsNewest,
                HasPromotion = p.Promotion != null
            }).ToList();

            var catalogData = new ProductCatalogData
            {
                UpdatedAt = DateTime.UtcNow,
                TotalProducts = productSummaries.Count,
                Products = productSummaries
            };

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var jsonData = JsonSerializer.Serialize(catalogData, jsonOptions);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = CacheExpiration
            };

            await _cache.SetStringAsync(CacheKey, jsonData, cacheOptions, cancellationToken);

            _logger.LogInformation(":::[Service:{ServiceName}]::: Product catalog cache refreshed with {ProductCount} products",
                nameof(ProductCatalogCacheService), productSummaries.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ":::[Service:{ServiceName}][Exception:{ExceptionType}]::: Error setting product catalog cache",
                nameof(ProductCatalogCacheService), ex.GetType().Name);
            throw;
        }
    }

    private static string GetPriceRange(List<SkuPriceList> prices)
    {
        if (prices == null || !prices.Any())
            return "Liên hệ";

        var allPrices = prices.Select(p => p.UnitPrice).ToList();

        if (!allPrices.Any())
            return "Liên hệ";

        var minPrice = allPrices.Min();
        var maxPrice = allPrices.Max();

        if (minPrice == maxPrice)
            return $"{minPrice:N0}đ";

        return $"{minPrice:N0}đ - {maxPrice:N0}đ";
    }

    private static string TruncateDescription(string? description, int maxLength)
    {
        if (string.IsNullOrEmpty(description))
            return "";

        if (description.Length <= maxLength)
            return description;

        return description.Substring(0, maxLength) + "...";
    }

    private class ProductCatalogData
    {
        public DateTime UpdatedAt { get; set; }
        public int TotalProducts { get; set; }
        public List<ProductSummary> Products { get; set; } = new();
    }

    private class ProductSummary
    {
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public List<string> Models { get; set; } = new();
        public List<string> Colors { get; set; } = new();
        public List<string> Storages { get; set; } = new();
        public string PriceRange { get; set; } = "";
        public string Description { get; set; } = "";
        public double AverageRating { get; set; }
        public int OverallSold { get; set; }
        public bool IsNewest { get; set; }
        public bool HasPromotion { get; set; }
    }
}
