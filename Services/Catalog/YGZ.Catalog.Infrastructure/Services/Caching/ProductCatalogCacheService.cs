using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using YGZ.Catalog.Application.Abstractions.Caching;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.ProductModels;
using YGZ.Catalog.Domain.Products.ProductModels.ValueObjects;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Services.Caching;

public class ProductCatalogCacheService : IProductCatalogCacheService
{
    private const string CacheKey = "chatbot:product_catalog_summary";
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromHours(24);

    private readonly IDistributedCache _cache;
    private readonly IMongoRepository<ProductModel, ModelId> _productModelRepository;
    private readonly IMongoRepository<SKU, SkuId> _skuRepository;
    private readonly IMongoRepository<Branch, BranchId> _branchRepository;
    private readonly ILogger<ProductCatalogCacheService> _logger;

    public ProductCatalogCacheService(
        IDistributedCache cache,
        IMongoRepository<ProductModel, ModelId> productModelRepository,
        IMongoRepository<SKU, SkuId> skuRepository,
        IMongoRepository<Branch, BranchId> branchRepository,
        ILogger<ProductCatalogCacheService> logger)
    {
        _cache = cache;
        _productModelRepository = productModelRepository;
        _skuRepository = skuRepository;
        _branchRepository = branchRepository;
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

            // Fetch products
            var (products, _, _) = await _productModelRepository.GetAllAsync(
                _page: 1,
                _limit: 100,
                filter: null,
                sort: null,
                cancellationToken: cancellationToken);

            // Fetch branches
            var branches = await _branchRepository.GetAllAsync(cancellationToken);
            var branchLookup = branches.ToDictionary(b => b.Id.Value!, b => b);

            // Fetch SKUs with available stock
            var skus = await _skuRepository.GetAllAsync(cancellationToken);
            var availableSkus = skus.Where(s => s.AvailableInStock > 0 && !s.IsDeleted).ToList();

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

            var branchSummaries = branches
                .Where(b => !b.IsDeleted)
                .Select(b => new BranchSummary
                {
                    Name = b.Name,
                    Address = b.Address
                }).ToList();

            var inventorySummaries = availableSkus
                .Where(s => branchLookup.ContainsKey(s.BranchId.Value!))
                .Select(s =>
                {
                    var branch = branchLookup[s.BranchId.Value!];
                    return new InventorySummary
                    {
                        ProductName = $"{s.Model.Name} {s.Color.Name} {s.Storage.Name}",
                        Model = s.Model.Name,
                        Color = s.Color.Name,
                        Storage = s.Storage.Name,
                        BranchName = branch.Name,
                        BranchAddress = branch.Address,
                        AvailableStock = s.AvailableInStock,
                        UnitPrice = s.UnitPrice,
                        IsOnPromotion = s.ReservedForEvent != null,
                        PromotionName = s.ReservedForEvent?.EventName
                    };
                }).ToList();

            // Get products on promotion/discount
            var promotionSummaries = availableSkus
                .Where(s => s.ReservedForEvent != null && branchLookup.ContainsKey(s.BranchId.Value!))
                .Select(s =>
                {
                    var branch = branchLookup[s.BranchId.Value!];
                    return new PromotionSummary
                    {
                        ProductName = $"{s.Model.Name} {s.Color.Name} {s.Storage.Name}",
                        EventName = s.ReservedForEvent!.EventName,
                        BranchName = branch.Name,
                        AvailableStock = s.ReservedForEvent.ReservedQuantity,
                        UnitPrice = s.UnitPrice
                    };
                }).ToList();

            var catalogData = new ProductCatalogData
            {
                UpdatedAt = DateTime.UtcNow,
                TotalProducts = productSummaries.Count,
                Products = productSummaries,
                Branches = branchSummaries,
                Inventory = inventorySummaries,
                Promotions = promotionSummaries
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

            _logger.LogInformation(":::[Service:{ServiceName}]::: Product catalog cache refreshed with {ProductCount} products, {BranchCount} branches, {InventoryCount} inventory items, {PromotionCount} promotions",
                nameof(ProductCatalogCacheService), productSummaries.Count, branchSummaries.Count, inventorySummaries.Count, promotionSummaries.Count);
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
            return $"${minPrice:N0}";

        return $"${minPrice:N0} - ${maxPrice:N0}";
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
        public List<BranchSummary> Branches { get; set; } = new();
        public List<InventorySummary> Inventory { get; set; } = new();
        public List<PromotionSummary> Promotions { get; set; } = new();
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

    private class BranchSummary
    {
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
    }

    private class InventorySummary
    {
        public string ProductName { get; set; } = "";
        public string Model { get; set; } = "";
        public string Color { get; set; } = "";
        public string Storage { get; set; } = "";
        public string BranchName { get; set; } = "";
        public string BranchAddress { get; set; } = "";
        public int AvailableStock { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsOnPromotion { get; set; }
        public string? PromotionName { get; set; }
    }

    private class PromotionSummary
    {
        public string ProductName { get; set; } = "";
        public string EventName { get; set; } = "";
        public string BranchName { get; set; } = "";
        public int AvailableStock { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
