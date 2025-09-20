# Promotion Strategies Design

## Strategy Overview

### Strategy 1: Special Event Discounts (Black Friday, etc.)

**Use Case**: Discount products during special promotional events
**Entities**: `PromotionEvents` → `PromotionGlobals` → `PromotionProducts/Categories`

### Strategy 2: Item-Specific Discounts

**Use Case**: Apply discounts to specific products individually
**Entities**: `PromotionItems`

### Strategy 3: Coupon-Based Discounts

**Use Case**: Apply discount codes to items
**Entities**: `Coupons`

## Current Schema Analysis

### ✅ What Works Well

-   Clear separation of concerns with different entity types
-   Proper audit trail and soft delete patterns
-   Flexible discount types (percentage, fixed amount)
-   Date range validation support

### ❌ Critical Issues

1. **Missing Relationships**: `PromotionItems` not linked to `PromotionEvents`
2. **Inconsistent ID Types**: Mixed UUID/text primary keys
3. **Missing Indexes**: Performance issues for queries
4. **No Conflict Resolution**: Multiple promotions can overlap
5. **Limited Product Targeting**: No category-level promotions in current design

## Recommended Schema Improvements

### 1. Fix Missing Relationships

```sql
-- Add relationship between PromotionItems and PromotionEvents
ALTER TABLE public."PromotionItems"
ADD COLUMN "PromotionEventId" uuid NULL;

ALTER TABLE public."PromotionItems"
ADD CONSTRAINT "FK_PromotionItems_PromotionEvents_PromotionEventId"
FOREIGN KEY ("PromotionEventId") REFERENCES public."PromotionEvents"("Id") ON DELETE SET NULL;
```

### 2. Standardize ID Types

```sql
-- Convert text IDs to UUID for consistency
ALTER TABLE public."PromotionProducts"
ALTER COLUMN "Id" TYPE uuid USING gen_random_uuid();

ALTER TABLE public."PromotionCategories"
ALTER COLUMN "Id" TYPE uuid USING gen_random_uuid();
```

### 3. Add Performance Indexes

```sql
-- Critical indexes for promotion queries
CREATE INDEX "IX_Coupons_Code_Active" ON public."Coupons" ("Code") WHERE "IsDeleted" = false;
CREATE INDEX "IX_Coupons_ValidPeriod" ON public."Coupons" ("ValidFrom", "ValidTo") WHERE "IsDeleted" = false;
CREATE INDEX "IX_PromotionItems_ProductId" ON public."PromotionItems" ("ProductId") WHERE "IsDeleted" = false;
CREATE INDEX "IX_PromotionItems_EventId" ON public."PromotionItems" ("PromotionEventId") WHERE "IsDeleted" = false;
CREATE INDEX "IX_PromotionItems_ValidPeriod" ON public."PromotionItems" ("ValidFrom", "ValidTo") WHERE "IsDeleted" = false;
CREATE INDEX "IX_PromotionEvents_ValidPeriod" ON public."PromotionEvents" ("ValidFrom", "ValidTo") WHERE "IsDeleted" = false;
```

### 4. Add Business Constraints

```sql
-- Ensure valid date ranges
ALTER TABLE public."Coupons"
ADD CONSTRAINT "CK_Coupons_ValidDateRange"
CHECK ("ValidTo" IS NULL OR "ValidTo" > "ValidFrom");

ALTER TABLE public."PromotionItems"
ADD CONSTRAINT "CK_PromotionItems_ValidDateRange"
CHECK ("ValidTo" IS NULL OR "ValidTo" > "ValidFrom");

-- Ensure positive discount values
ALTER TABLE public."Coupons"
ADD CONSTRAINT "CK_Coupons_PositiveDiscount"
CHECK ("DiscountValue" > 0);

ALTER TABLE public."PromotionItems"
ADD CONSTRAINT "CK_PromotionItems_PositiveDiscount"
CHECK ("DiscountValue" > 0);

-- Ensure unique coupon codes
ALTER TABLE public."Coupons"
ADD CONSTRAINT "UQ_Coupons_Code" UNIQUE ("Code");
```

## Implementation Strategy

### Strategy 1: Event-Based Promotions (Black Friday)

```csharp
// Example: Create Black Friday event with multiple product discounts
var blackFridayEvent = PromotionEvent.Create(
    id: new PromotionEventId(),
    title: "Black Friday 2024",
    description: "Biggest sale of the year",
    discountState: DiscountState.ACTIVE,
    validFrom: new DateTime(2024, 11, 24),
    validTo: new DateTime(2024, 11, 26)
);

// Add global promotions to the event
var globalPromotion = PromotionGlobal.Create(
    id: new PromotionGlobalId(),
    title: "Black Friday Electronics",
    description: "50% off all electronics",
    promotionGlobalType: PromotionGlobalType.PRODUCT_CATEGORY,
    promotionEventId: blackFridayEvent.Id
);
```

### Strategy 2: Item-Specific Promotions

```csharp
// Example: Specific iPhone discount
var iphonePromotion = PromotionItem.Create(
    id: new PromotionItemId(),
    productId: new ProductId("iphone-15-pro"),
    title: "iPhone 15 Pro Flash Sale",
    description: "Limited time offer",
    discountState: DiscountState.ACTIVE,
    discountType: DiscountType.PERCENTAGE,
    endDiscountType: EndDiscountType.BY_END_DATE,
    discountValue: 15.0m, // 15% off
    productNameTag: new ProductNameTag("iPhone"),
    validFrom: DateTime.UtcNow,
    validTo: DateTime.UtcNow.AddDays(7),
    availableQuantity: 100,
    productImage: "iphone-15-pro.jpg",
    productSlug: "iphone-15-pro"
);
```

### Strategy 3: Coupon-Based Promotions

```csharp
// Example: Welcome coupon for new customers
var welcomeCoupon = Coupon.Create(
    id: new CouponId(),
    code: new Code("WELCOME20"),
    title: "Welcome Discount",
    description: "20% off your first purchase",
    productNameTag: new ProductNameTag("All Products"),
    promotionEventType: PromotionEventType.PROMOTION_COUPON,
    discountState: DiscountState.ACTIVE,
    discountType: DiscountType.PERCENTAGE,
    discountValue: 20.0m,
    maxDiscountAmount: 100.0m, // Max $100 discount
    validFrom: DateTime.UtcNow,
    validTo: DateTime.UtcNow.AddMonths(3),
    availableQuantity: 1000
);
```

## Promotion Resolution Logic

### Priority Order (Highest to Lowest)

1. **Item-Specific Promotions** - Most specific, highest priority
2. **Event-Based Promotions** - Category/product level
3. **Coupon-Based Promotions** - User-initiated, lowest priority

### Query Strategy

```sql
-- Get all applicable promotions for a product
WITH applicable_promotions AS (
    -- Strategy 2: Item-specific promotions
    SELECT 'ITEM_SPECIFIC' as promotion_type, discount_value, discount_type
    FROM "PromotionItems"
    WHERE product_id = @ProductId
      AND discount_state = 'ACTIVE'
      AND (valid_from IS NULL OR valid_from <= @CurrentDate)
      AND (valid_to IS NULL OR valid_to >= @CurrentDate)
      AND (available_quantity IS NULL OR available_quantity > 0)
      AND is_deleted = false

    UNION ALL

    -- Strategy 1: Event-based promotions
    SELECT 'EVENT_BASED' as promotion_type, pp.discount_value, pp.discount_type
    FROM "PromotionProducts" pp
    JOIN "PromotionGlobals" pg ON pp.promotion_global_id = pg.id
    JOIN "PromotionEvents" pe ON pg.promotion_event_id = pe.id
    WHERE pp.product_slug = @ProductSlug
      AND pe.discount_state = 'ACTIVE'
      AND (pe.valid_from IS NULL OR pe.valid_from <= @CurrentDate)
      AND (pe.valid_to IS NULL OR pe.valid_to >= @CurrentDate)
      AND pe.is_deleted = false
      AND pg.is_deleted = false
      AND pp.is_deleted = false
)
SELECT * FROM applicable_promotions
ORDER BY
    CASE promotion_type
        WHEN 'ITEM_SPECIFIC' THEN 1
        WHEN 'EVENT_BASED' THEN 2
        ELSE 3
    END
LIMIT 1;
```

## API Design Recommendations

### Endpoints Structure

```
GET /api/promotions/products/{productId}/applicable
GET /api/promotions/coupons/validate/{code}
GET /api/promotions/events/active
POST /api/promotions/coupons/redeem
```

### Response Models

```csharp
public record ApplicablePromotionResponse
{
    public string PromotionType { get; init; } // "ITEM_SPECIFIC", "EVENT_BASED", "COUPON"
    public decimal DiscountValue { get; init; }
    public string DiscountType { get; init; } // "PERCENTAGE", "FIXED_AMOUNT"
    public decimal? MaxDiscountAmount { get; init; }
    public DateTime? ValidTo { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
}
```

## Performance Considerations

### Caching Strategy

-   Cache active promotions by product ID
-   Cache coupon validation results
-   Use Redis for high-frequency lookups

### Database Optimization

-   Partition large tables by date ranges
-   Consider read replicas for promotion queries
-   Implement proper connection pooling

## Monitoring & Analytics

### Key Metrics

-   Promotion redemption rates
-   Revenue impact per promotion
-   Customer engagement metrics
-   Promotion conflict resolution events

### Alerting

-   Promotion overlap detection
-   High-value promotion abuse
-   System performance degradation
