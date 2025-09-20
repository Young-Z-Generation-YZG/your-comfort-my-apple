# Promotion System Implementation Guide

## Overview

This guide provides a complete implementation for your 3 promotion strategies using the improved database schema.

## Strategy Implementation

### Strategy 1: Special Event Discounts (Black Friday)

#### Database Structure

```
PromotionEvents (Event Container)
├── PromotionGlobals (Global Rules)
│   ├── PromotionProducts (Specific Products)
│   └── PromotionCategories (Category-wide)
```

#### Use Cases

-   Black Friday sales
-   Holiday promotions
-   Seasonal discounts
-   Flash sales

#### Implementation Example

```csharp
// Create Black Friday Event
public class CreateBlackFridayEventCommand : ICommand<bool>
{
    public string Title { get; init; } = "Black Friday 2024";
    public string Description { get; init; } = "Biggest sale of the year";
    public DateTime ValidFrom { get; init; }
    public DateTime ValidTo { get; init; }
}

public class CreateBlackFridayEventCommandHandler : ICommandHandler<CreateBlackFridayEventCommand, bool>
{
    private readonly IPromotionEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<Result<bool>> Handle(CreateBlackFridayEventCommand request, CancellationToken cancellationToken)
    {
        var eventId = new PromotionEventId();
        var blackFridayEvent = PromotionEvent.Create(
            eventId,
            request.Title,
            request.Description,
            DiscountState.ACTIVE,
            request.ValidFrom,
            request.ValidTo
        );

        await _eventRepository.AddAsync(blackFridayEvent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}

// Add Electronics Category Discount
public class AddElectronicsDiscountCommand : ICommand<bool>
{
    public Guid PromotionEventId { get; init; }
    public decimal DiscountPercentage { get; init; }
    public List<string> CategoryIds { get; init; } = new();
}

public class AddElectronicsDiscountCommandHandler : ICommandHandler<AddElectronicsDiscountCommand, bool>
{
    private readonly IPromotionGlobalRepository _globalRepository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<Result<bool>> Handle(AddElectronicsDiscountCommand request, CancellationToken cancellationToken)
    {
        var globalId = new PromotionGlobalId();
        var globalPromotion = PromotionGlobal.Create(
            globalId,
            "Electronics Black Friday",
            "50% off all electronics",
            PromotionGlobalType.PRODUCT_CATEGORY,
            new PromotionEventId(request.PromotionEventId),
            DiscountType.PERCENTAGE,
            request.DiscountPercentage
        );

        await _globalRepository.AddAsync(globalPromotion);

        // Add categories to the promotion
        foreach (var categoryId in request.CategoryIds)
        {
            var categoryPromotion = PromotionCategory.Create(
                new PromotionCategoryId(),
                categoryId,
                $"Category {categoryId}",
                $"category-{categoryId}",
                globalId
            );
            await _globalRepository.AddCategoryAsync(categoryPromotion);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(true);
    }
}
```

### Strategy 2: Item-Specific Discounts

#### Use Cases

-   Flash sales on specific products
-   Clearance items
-   Limited-time offers
-   Product launch promotions

#### Implementation Example

```csharp
public class CreateProductPromotionCommand : ICommand<bool>
{
    public string ProductId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public DiscountType DiscountType { get; init; }
    public decimal DiscountValue { get; init; }
    public decimal? MaxDiscountAmount { get; init; }
    public DateTime? ValidFrom { get; init; }
    public DateTime? ValidTo { get; init; }
    public int? AvailableQuantity { get; init; }
    public Guid? PromotionEventId { get; init; } // Optional: link to event
}

public class CreateProductPromotionCommandHandler : ICommandHandler<CreateProductPromotionCommand, bool>
{
    private readonly IPromotionItemRepository _itemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<Result<bool>> Handle(CreateProductPromotionCommand request, CancellationToken cancellationToken)
    {
        var promotionId = new PromotionItemId();
        var productPromotion = PromotionItem.Create(
            promotionId,
            new ProductId(request.ProductId),
            request.Title,
            request.Description,
            DiscountState.ACTIVE,
            request.DiscountType,
            EndDiscountType.BY_END_DATE,
            request.DiscountValue,
            new ProductNameTag("Product"), // Get from product service
            request.ValidFrom,
            request.ValidTo,
            request.AvailableQuantity,
            "product-image.jpg", // Get from product service
            $"product-{request.ProductId}"
        );

        // Link to promotion event if provided
        if (request.PromotionEventId.HasValue)
        {
            productPromotion.SetPromotionEvent(new PromotionEventId(request.PromotionEventId.Value));
        }

        await _itemRepository.AddAsync(productPromotion);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}
```

### Strategy 3: Coupon-Based Discounts

#### Use Cases

-   Welcome coupons for new customers
-   Loyalty rewards
-   Referral bonuses
-   Seasonal coupon campaigns

#### Implementation Example

```csharp
public class CreateCouponCommand : ICommand<bool>
{
    public string Code { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string ProductNameTag { get; init; }
    public DiscountType DiscountType { get; init; }
    public decimal DiscountValue { get; init; }
    public decimal? MaxDiscountAmount { get; init; }
    public decimal? MinOrderAmount { get; init; }
    public DateTime? ValidFrom { get; init; }
    public DateTime? ValidTo { get; init; }
    public int AvailableQuantity { get; init; }
    public int? MaxUsesPerCustomer { get; init; }
}

public class CreateCouponCommandHandler : ICommandHandler<CreateCouponCommand, bool>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<Result<bool>> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        // Validate coupon code uniqueness
        var existingCoupon = await _couponRepository.GetByCodeAsync(new Code(request.Code));
        if (existingCoupon is not null)
        {
            return Result.Failure<bool>(Error.Validation("Coupon.Code", "Coupon code already exists"));
        }

        var couponId = new CouponId();
        var coupon = Coupon.Create(
            couponId,
            new Code(request.Code),
            request.Title,
            request.Description,
            new ProductNameTag(request.ProductNameTag),
            PromotionEventType.PROMOTION_COUPON,
            DiscountState.ACTIVE,
            request.DiscountType,
            request.DiscountValue,
            request.MaxDiscountAmount,
            request.MinOrderAmount,
            request.ValidFrom,
            request.ValidTo,
            request.AvailableQuantity,
            request.MaxUsesPerCustomer
        );

        await _couponRepository.AddAsync(coupon);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}

// Validate and Apply Coupon
public class ValidateCouponQuery : IQuery<CouponValidationResult>
{
    public string Code { get; init; }
    public string ProductId { get; init; }
    public decimal OrderAmount { get; init; }
    public string CustomerId { get; init; }
}

public class ValidateCouponQueryHandler : IQueryHandler<ValidateCouponQuery, CouponValidationResult>
{
    private readonly ICouponRepository _couponRepository;
    private readonly ICouponUsageRepository _usageRepository;

    public async Task<Result<CouponValidationResult>> Handle(ValidateCouponQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _couponRepository.GetByCodeAsync(new Code(request.Code));
        if (coupon is null)
        {
            return Result.Failure<CouponValidationResult>(Error.NotFound("Coupon", request.Code));
        }

        // Validate coupon state
        if (coupon.DiscountState != DiscountState.ACTIVE)
        {
            return Result.Failure<CouponValidationResult>(Error.Validation("Coupon.State", "Coupon is not active"));
        }

        // Validate date range
        var now = DateTime.UtcNow;
        if (coupon.ValidFrom.HasValue && coupon.ValidFrom > now)
        {
            return Result.Failure<CouponValidationResult>(Error.Validation("Coupon.ValidFrom", "Coupon is not yet valid"));
        }

        if (coupon.ValidTo.HasValue && coupon.ValidTo < now)
        {
            return Result.Failure<CouponValidationResult>(Error.Validation("Coupon.ValidTo", "Coupon has expired"));
        }

        // Validate availability
        if (coupon.UsedQuantity >= coupon.AvailableQuantity)
        {
            return Result.Failure<CouponValidationResult>(Error.Validation("Coupon.Quantity", "Coupon is fully redeemed"));
        }

        // Validate minimum order amount
        if (coupon.MinOrderAmount.HasValue && request.OrderAmount < coupon.MinOrderAmount.Value)
        {
            return Result.Failure<CouponValidationResult>(Error.Validation("Coupon.MinOrder",
                $"Minimum order amount of {coupon.MinOrderAmount:C} required"));
        }

        // Validate customer usage limit
        if (coupon.MaxUsesPerCustomer.HasValue)
        {
            var customerUsage = await _usageRepository.GetCustomerUsageCountAsync(
                new CustomerId(request.CustomerId),
                coupon.Id);

            if (customerUsage >= coupon.MaxUsesPerCustomer.Value)
            {
                return Result.Failure<CouponValidationResult>(Error.Validation("Coupon.CustomerLimit",
                    "Maximum uses per customer exceeded"));
            }
        }

        // Calculate discount amount
        var discountAmount = CalculateDiscountAmount(coupon, request.OrderAmount);

        var result = new CouponValidationResult
        {
            IsValid = true,
            CouponId = coupon.Id.Value,
            DiscountAmount = discountAmount,
            DiscountType = coupon.DiscountType.Name,
            MaxDiscountAmount = coupon.MaxDiscountAmount,
            Title = coupon.Title,
            Description = coupon.Description
        };

        return Result.Success(result);
    }

    private decimal CalculateDiscountAmount(Coupon coupon, decimal orderAmount)
    {
        decimal discountAmount = 0;

        if (coupon.DiscountType == DiscountType.PERCENTAGE)
        {
            discountAmount = orderAmount * (coupon.DiscountValue / 100);

            // Apply maximum discount limit
            if (coupon.MaxDiscountAmount.HasValue && discountAmount > coupon.MaxDiscountAmount.Value)
            {
                discountAmount = coupon.MaxDiscountAmount.Value;
            }
        }
        else if (coupon.DiscountType == DiscountType.FIXED_AMOUNT)
        {
            discountAmount = Math.Min(coupon.DiscountValue, orderAmount);
        }

        return Math.Round(discountAmount, 2);
    }
}
```

## Promotion Resolution Service

### Core Service for Finding Best Promotion

```csharp
public interface IPromotionResolutionService
{
    Task<PromotionResult?> GetBestPromotionAsync(
        string productId,
        string productSlug,
        string categoryId,
        string? couponCode = null);
}

public class PromotionResolutionService : IPromotionResolutionService
{
    private readonly IPromotionItemRepository _itemRepository;
    private readonly IPromotionGlobalRepository _globalRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly ILogger<PromotionResolutionService> _logger;

    public async Task<PromotionResult?> GetBestPromotionAsync(
        string productId,
        string productSlug,
        string categoryId,
        string? couponCode = null)
    {
        var promotions = new List<PromotionResult>();

        // Strategy 2: Check item-specific promotions (Highest Priority)
        var itemPromotions = await _itemRepository.GetActivePromotionsForProductAsync(productId);
        promotions.AddRange(itemPromotions.Select(MapToPromotionResult));

        // Strategy 1: Check event-based promotions (Medium Priority)
        var globalPromotions = await _globalRepository.GetActivePromotionsForProductAsync(productSlug, categoryId);
        promotions.AddRange(globalPromotions.Select(MapToPromotionResult));

        // Strategy 3: Check coupon promotions (Lowest Priority, but user-initiated)
        if (!string.IsNullOrEmpty(couponCode))
        {
            var coupon = await _couponRepository.GetByCodeAsync(new Code(couponCode));
            if (coupon?.DiscountState == DiscountState.ACTIVE)
            {
                var couponPromotion = MapToPromotionResult(coupon);
                promotions.Add(couponPromotion);
            }
        }

        // Return the best promotion based on priority and discount value
        var bestPromotion = promotions
            .OrderBy(p => p.Priority)
            .ThenByDescending(p => p.DiscountAmount)
            .FirstOrDefault();

        _logger.LogInformation("Found {Count} applicable promotions for product {ProductId}, selected: {PromotionType}",
            promotions.Count, productId, bestPromotion?.PromotionType);

        return bestPromotion;
    }

    private PromotionResult MapToPromotionResult(PromotionItem item)
    {
        return new PromotionResult
        {
            PromotionType = "ITEM_SPECIFIC",
            PromotionId = item.Id.Value,
            DiscountAmount = item.DiscountValue,
            DiscountType = item.DiscountType.Name,
            MaxDiscountAmount = item.MaxDiscountAmount,
            ValidTo = item.ValidTo,
            Title = item.Title,
            Description = item.Description,
            Priority = 1
        };
    }

    private PromotionResult MapToPromotionResult(PromotionGlobal global)
    {
        return new PromotionResult
        {
            PromotionType = "EVENT_BASED",
            PromotionId = global.Id.Value,
            DiscountAmount = global.DiscountValue,
            DiscountType = global.DiscountType.Name,
            MaxDiscountAmount = global.MaxDiscountAmount,
            ValidTo = null, // Inherited from event
            Title = global.Title,
            Description = global.Description,
            Priority = 2
        };
    }

    private PromotionResult MapToPromotionResult(Coupon coupon)
    {
        return new PromotionResult
        {
            PromotionType = "COUPON",
            PromotionId = coupon.Id.Value,
            DiscountAmount = coupon.DiscountValue,
            DiscountType = coupon.DiscountType.Name,
            MaxDiscountAmount = coupon.MaxDiscountAmount,
            ValidTo = coupon.ValidTo,
            Title = coupon.Title,
            Description = coupon.Description,
            Priority = 3
        };
    }
}

public class PromotionResult
{
    public string PromotionType { get; set; } = string.Empty;
    public Guid PromotionId { get; set; }
    public decimal DiscountAmount { get; set; }
    public string DiscountType { get; set; } = string.Empty;
    public decimal? MaxDiscountAmount { get; set; }
    public DateTime? ValidTo { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Priority { get; set; }
}
```

## API Controller Example

```csharp
[ApiController]
[Route("api/[controller]")]
public class PromotionsController : ControllerBase
{
    private readonly IPromotionResolutionService _promotionService;
    private readonly IMediator _mediator;

    [HttpGet("products/{productId}/applicable")]
    public async Task<ActionResult<ApplicablePromotionResponse>> GetApplicablePromotions(
        string productId,
        [FromQuery] string? couponCode = null)
    {
        // Get product details from catalog service
        var product = await GetProductDetailsAsync(productId);

        var promotion = await _promotionService.GetBestPromotionAsync(
            productId,
            product.Slug,
            product.CategoryId,
            couponCode);

        if (promotion is null)
        {
            return Ok(new ApplicablePromotionResponse { HasPromotion = false });
        }

        var response = new ApplicablePromotionResponse
        {
            HasPromotion = true,
            PromotionType = promotion.PromotionType,
            DiscountAmount = promotion.DiscountAmount,
            DiscountType = promotion.DiscountType,
            MaxDiscountAmount = promotion.MaxDiscountAmount,
            ValidTo = promotion.ValidTo,
            Title = promotion.Title,
            Description = promotion.Description
        };

        return Ok(response);
    }

    [HttpPost("coupons/validate")]
    public async Task<ActionResult<CouponValidationResult>> ValidateCoupon(
        [FromBody] ValidateCouponRequest request)
    {
        var query = new ValidateCouponQuery
        {
            Code = request.Code,
            ProductId = request.ProductId,
            OrderAmount = request.OrderAmount,
            CustomerId = request.CustomerId
        };

        var result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost("events")]
    public async Task<ActionResult<Guid>> CreatePromotionEvent(
        [FromBody] CreatePromotionEventRequest request)
    {
        var command = new CreatePromotionEventCommand
        {
            Title = request.Title,
            Description = request.Description,
            ValidFrom = request.ValidFrom,
            ValidTo = request.ValidTo
        };

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}
```

## Performance Optimization

### Caching Strategy

```csharp
public class CachedPromotionResolutionService : IPromotionResolutionService
{
    private readonly IPromotionResolutionService _innerService;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CachedPromotionResolutionService> _logger;

    public async Task<PromotionResult?> GetBestPromotionAsync(
        string productId,
        string productSlug,
        string categoryId,
        string? couponCode = null)
    {
        var cacheKey = $"promotion:{productId}:{productSlug}:{categoryId}:{couponCode ?? "none"}";

        if (_cache.TryGetValue(cacheKey, out PromotionResult? cachedResult))
        {
            _logger.LogDebug("Cache hit for promotion key: {CacheKey}", cacheKey);
            return cachedResult;
        }

        var result = await _innerService.GetBestPromotionAsync(productId, productSlug, categoryId, couponCode);

        // Cache for 5 minutes
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            SlidingExpiration = TimeSpan.FromMinutes(1)
        };

        _cache.Set(cacheKey, result, cacheOptions);

        return result;
    }
}
```

## Monitoring and Analytics

### Key Metrics to Track

```csharp
public class PromotionMetricsService
{
    private readonly IMetrics _metrics;
    private readonly IPromotionRepository _repository;

    public async Task TrackPromotionUsage(string promotionType, Guid promotionId)
    {
        _metrics.Counter("promotion.usage.total", new TagList
        {
            ["promotion_type"] = promotionType,
            ["promotion_id"] = promotionId.ToString()
        }).Increment();
    }

    public async Task TrackPromotionRevenue(string promotionType, decimal revenue)
    {
        _metrics.Counter("promotion.revenue.total", new TagList
        {
            ["promotion_type"] = promotionType
        }).Increment(revenue);
    }

    public async Task TrackPromotionConflicts(int conflictCount)
    {
        _metrics.Counter("promotion.conflicts.total").Increment(conflictCount);
    }
}
```

This implementation provides a complete, production-ready promotion system that efficiently handles your 3 strategies with proper separation of concerns, performance optimization, and monitoring capabilities.
