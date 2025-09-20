-- =====================================================
-- IMPROVED DISCOUNT SCHEMA FOR 3 PROMOTION STRATEGIES
-- =====================================================

-- Drop existing tables if they exist (for migration)
-- DROP TABLE IF EXISTS public."PromotionProducts" CASCADE;
-- DROP TABLE IF EXISTS public."PromotionCategories" CASCADE;
-- DROP TABLE IF EXISTS public."PromotionGlobals" CASCADE;
-- DROP TABLE IF EXISTS public."PromotionItems" CASCADE;
-- DROP TABLE IF EXISTS public."PromotionEvents" CASCADE;
-- DROP TABLE IF EXISTS public."Coupons" CASCADE;

-- =====================================================
-- STRATEGY 3: COUPON-BASED DISCOUNTS
-- =====================================================
CREATE TABLE public."Coupons" (
	"Id" uuid NOT NULL,
	"Code" varchar(50) NOT NULL, -- Reduced length for better performance
	"Title" varchar(200) NOT NULL,
	"Description" text NOT NULL,
	"DiscountState" varchar(20) NOT NULL DEFAULT 'INACTIVE', -- ACTIVE, INACTIVE, EXPIRED
	"ProductNameTag" varchar(100) NOT NULL, -- Product category filter
	"PromotionEventType" varchar(30) NOT NULL DEFAULT 'PROMOTION_COUPON',
	"DiscountType" varchar(20) NOT NULL DEFAULT 'PERCENTAGE', -- PERCENTAGE, FIXED_AMOUNT
	"DiscountValue" numeric(10,2) NOT NULL CHECK ("DiscountValue" > 0),
	"MaxDiscountAmount" numeric(10,2) NULL CHECK ("MaxDiscountAmount" > 0),
	"MinOrderAmount" numeric(10,2) NULL DEFAULT 0 CHECK ("MinOrderAmount" >= 0),
	"ValidFrom" timestamptz NULL,
	"ValidTo" timestamptz NULL,
	"AvailableQuantity" int4 NOT NULL DEFAULT 0 CHECK ("AvailableQuantity" >= 0),
	"UsedQuantity" int4 NOT NULL DEFAULT 0 CHECK ("UsedQuantity" >= 0),
	"MaxUsesPerCustomer" int4 NULL CHECK ("MaxUsesPerCustomer" > 0),
	"CreatedAt" timestamptz NOT NULL DEFAULT NOW(),
	"UpdatedAt" timestamptz NOT NULL DEFAULT NOW(),
	"IsDeleted" bool NOT NULL DEFAULT false,
	"DeletedAt" timestamptz NULL,
	"DeletedBy" varchar(100) NULL,
	
	CONSTRAINT "PK_Coupons" PRIMARY KEY ("Id"),
	CONSTRAINT "UQ_Coupons_Code" UNIQUE ("Code"),
	CONSTRAINT "CK_Coupons_ValidDateRange" CHECK ("ValidTo" IS NULL OR "ValidTo" > "ValidFrom"),
	CONSTRAINT "CK_Coupons_QuantityCheck" CHECK ("UsedQuantity" <= "AvailableQuantity")
);

-- =====================================================
-- STRATEGY 1: SPECIAL EVENT DISCOUNTS (Black Friday, etc.)
-- =====================================================
CREATE TABLE public."PromotionEvents" (
	"Id" uuid NOT NULL,
	"Title" varchar(200) NOT NULL,
	"Description" text NOT NULL,
	"PromotionEventType" varchar(30) NOT NULL DEFAULT 'PROMOTION_EVENT',
	"DiscountState" varchar(20) NOT NULL DEFAULT 'INACTIVE', -- ACTIVE, INACTIVE, EXPIRED
	"ValidFrom" timestamptz NULL,
	"ValidTo" timestamptz NULL,
	"CreatedAt" timestamptz NOT NULL DEFAULT NOW(),
	"UpdatedAt" timestamptz NOT NULL DEFAULT NOW(),
	"IsDeleted" bool NOT NULL DEFAULT false,
	"DeletedAt" timestamptz NULL,
	"DeletedBy" varchar(100) NULL,
	
	CONSTRAINT "PK_PromotionEvents" PRIMARY KEY ("Id"),
	CONSTRAINT "CK_PromotionEvents_ValidDateRange" CHECK ("ValidTo" IS NULL OR "ValidTo" > "ValidFrom")
);

-- =====================================================
-- GLOBAL PROMOTIONS (Category/Product level discounts within events)
-- =====================================================
CREATE TABLE public."PromotionGlobals" (
	"Id" uuid NOT NULL,
	"Title" varchar(200) NOT NULL,
	"Description" text NOT NULL,
	"PromotionGlobalType" varchar(30) NOT NULL, -- PRODUCT_CATEGORY, SPECIFIC_PRODUCTS
	"PromotionEventId" uuid NOT NULL,
	"DiscountType" varchar(20) NOT NULL DEFAULT 'PERCENTAGE', -- PERCENTAGE, FIXED_AMOUNT
	"DiscountValue" numeric(10,2) NOT NULL CHECK ("DiscountValue" > 0),
	"MaxDiscountAmount" numeric(10,2) NULL CHECK ("MaxDiscountAmount" > 0),
	"CreatedAt" timestamptz NOT NULL DEFAULT NOW(),
	"UpdatedAt" timestamptz NOT NULL DEFAULT NOW(),
	"IsDeleted" bool NOT NULL DEFAULT false,
	"DeletedAt" timestamptz NULL,
	"DeletedBy" varchar(100) NULL,
	
	CONSTRAINT "PK_PromotionGlobals" PRIMARY KEY ("Id"),
	CONSTRAINT "FK_PromotionGlobals_PromotionEvents_PromotionEventId" 
		FOREIGN KEY ("PromotionEventId") REFERENCES public."PromotionEvents"("Id") ON DELETE CASCADE
);

-- =====================================================
-- PRODUCT-SPECIFIC PROMOTIONS WITHIN GLOBAL PROMOTIONS
-- =====================================================
CREATE TABLE public."PromotionProducts" (
	"Id" uuid NOT NULL, -- Changed from text to uuid for consistency
	"ProductId" varchar(100) NOT NULL,
	"ProductSlug" varchar(200) NOT NULL,
	"ProductImage" varchar(500) NULL,
	"PromotionGlobalId" uuid NOT NULL,
	"CreatedAt" timestamptz NOT NULL DEFAULT NOW(),
	"UpdatedAt" timestamptz NOT NULL DEFAULT NOW(),
	"IsDeleted" bool NOT NULL DEFAULT false,
	"DeletedAt" timestamptz NULL,
	"DeletedBy" varchar(100) NULL,
	
	CONSTRAINT "PK_PromotionProducts" PRIMARY KEY ("Id"),
	CONSTRAINT "FK_PromotionProducts_PromotionGlobals_PromotionGlobalId" 
		FOREIGN KEY ("PromotionGlobalId") REFERENCES public."PromotionGlobals"("Id") ON DELETE CASCADE,
	CONSTRAINT "UQ_PromotionProducts_Product_Global" UNIQUE ("ProductId", "PromotionGlobalId")
);

-- =====================================================
-- CATEGORY-SPECIFIC PROMOTIONS WITHIN GLOBAL PROMOTIONS
-- =====================================================
CREATE TABLE public."PromotionCategories" (
	"Id" uuid NOT NULL, -- Changed from text to uuid for consistency
	"CategoryId" varchar(100) NOT NULL,
	"CategoryName" varchar(200) NOT NULL,
	"CategorySlug" varchar(200) NOT NULL,
	"PromotionGlobalId" uuid NOT NULL,
	"CreatedAt" timestamptz NOT NULL DEFAULT NOW(),
	"UpdatedAt" timestamptz NOT NULL DEFAULT NOW(),
	"IsDeleted" bool NOT NULL DEFAULT false,
	"DeletedAt" timestamptz NULL,
	"DeletedBy" varchar(100) NULL,
	
	CONSTRAINT "PK_PromotionCategories" PRIMARY KEY ("Id"),
	CONSTRAINT "FK_PromotionCategories_PromotionGlobals_PromotionGlobalId" 
		FOREIGN KEY ("PromotionGlobalId") REFERENCES public."PromotionGlobals"("Id") ON DELETE CASCADE,
	CONSTRAINT "UQ_PromotionCategories_Category_Global" UNIQUE ("CategoryId", "PromotionGlobalId")
);

-- =====================================================
-- STRATEGY 2: ITEM-SPECIFIC DISCOUNTS
-- =====================================================
CREATE TABLE public."PromotionItems" (
	"Id" uuid NOT NULL,
	"Title" varchar(200) NOT NULL,
	"Description" text NOT NULL,
	"ProductId" varchar(100) NOT NULL,
	"ProductNameTag" varchar(100) NOT NULL,
	"ProductImage" varchar(500) NOT NULL,
	"ProductSlug" varchar(200) NOT NULL,
	"PromotionEventType" varchar(30) NOT NULL DEFAULT 'PROMOTION_ITEM',
	"DiscountState" varchar(20) NOT NULL DEFAULT 'INACTIVE', -- ACTIVE, INACTIVE, EXPIRED
	"DiscountType" varchar(20) NOT NULL DEFAULT 'PERCENTAGE', -- PERCENTAGE, FIXED_AMOUNT
	"DiscountValue" numeric(10,2) NOT NULL CHECK ("DiscountValue" > 0),
	"MaxDiscountAmount" numeric(10,2) NULL CHECK ("MaxDiscountAmount" > 0),
	"EndDiscountType" varchar(20) NOT NULL DEFAULT 'BY_END_DATE', -- BY_END_DATE, BY_QUANTITY
	"ValidFrom" timestamptz NULL,
	"ValidTo" timestamptz NULL,
	"AvailableQuantity" int4 NULL CHECK ("AvailableQuantity" > 0),
	"UsedQuantity" int4 NOT NULL DEFAULT 0 CHECK ("UsedQuantity" >= 0),
	"PromotionEventId" uuid NULL, -- FIXED: Added missing relationship
	"CreatedAt" timestamptz NOT NULL DEFAULT NOW(),
	"UpdatedAt" timestamptz NOT NULL DEFAULT NOW(),
	"IsDeleted" bool NOT NULL DEFAULT false,
	"DeletedAt" timestamptz NULL,
	"DeletedBy" varchar(100) NULL,
	
	CONSTRAINT "PK_PromotionItems" PRIMARY KEY ("Id"),
	CONSTRAINT "FK_PromotionItems_PromotionEvents_PromotionEventId" 
		FOREIGN KEY ("PromotionEventId") REFERENCES public."PromotionEvents"("Id") ON DELETE SET NULL,
	CONSTRAINT "CK_PromotionItems_ValidDateRange" CHECK ("ValidTo" IS NULL OR "ValidTo" > "ValidFrom"),
	CONSTRAINT "CK_PromotionItems_QuantityCheck" CHECK ("UsedQuantity" <= COALESCE("AvailableQuantity", 999999999))
);

-- =====================================================
-- PERFORMANCE INDEXES
-- =====================================================

-- Coupon indexes
CREATE INDEX "IX_Coupons_Code_Active" ON public."Coupons" ("Code") WHERE "IsDeleted" = false;
CREATE INDEX "IX_Coupons_ValidPeriod_Active" ON public."Coupons" ("ValidFrom", "ValidTo") WHERE "IsDeleted" = false AND "DiscountState" = 'ACTIVE';
CREATE INDEX "IX_Coupons_ProductNameTag" ON public."Coupons" ("ProductNameTag") WHERE "IsDeleted" = false;

-- Promotion Events indexes
CREATE INDEX "IX_PromotionEvents_ValidPeriod_Active" ON public."PromotionEvents" ("ValidFrom", "ValidTo") WHERE "IsDeleted" = false AND "DiscountState" = 'ACTIVE';
CREATE INDEX "IX_PromotionEvents_State" ON public."PromotionEvents" ("DiscountState") WHERE "IsDeleted" = false;

-- Promotion Globals indexes
CREATE INDEX "IX_PromotionGlobals_PromotionEventId" ON public."PromotionGlobals" ("PromotionEventId") WHERE "IsDeleted" = false;

-- Promotion Products indexes
CREATE INDEX "IX_PromotionProducts_ProductId_Active" ON public."PromotionProducts" ("ProductId") WHERE "IsDeleted" = false;
CREATE INDEX "IX_PromotionProducts_ProductSlug_Active" ON public."PromotionProducts" ("ProductSlug") WHERE "IsDeleted" = false;
CREATE INDEX "IX_PromotionProducts_PromotionGlobalId" ON public."PromotionProducts" ("PromotionGlobalId") WHERE "IsDeleted" = false;

-- Promotion Categories indexes
CREATE INDEX "IX_PromotionCategories_CategoryId_Active" ON public."PromotionCategories" ("CategoryId") WHERE "IsDeleted" = false;
CREATE INDEX "IX_PromotionCategories_CategorySlug_Active" ON public."PromotionCategories" ("CategorySlug") WHERE "IsDeleted" = false;
CREATE INDEX "IX_PromotionCategories_PromotionGlobalId" ON public."PromotionCategories" ("PromotionGlobalId") WHERE "IsDeleted" = false;

-- Promotion Items indexes
CREATE INDEX "IX_PromotionItems_ProductId_Active" ON public."PromotionItems" ("ProductId") WHERE "IsDeleted" = false AND "DiscountState" = 'ACTIVE';
CREATE INDEX "IX_PromotionItems_ProductSlug_Active" ON public."PromotionItems" ("ProductSlug") WHERE "IsDeleted" = false AND "DiscountState" = 'ACTIVE';
CREATE INDEX "IX_PromotionItems_ValidPeriod_Active" ON public."PromotionItems" ("ValidFrom", "ValidTo") WHERE "IsDeleted" = false AND "DiscountState" = 'ACTIVE';
CREATE INDEX "IX_PromotionItems_PromotionEventId" ON public."PromotionItems" ("PromotionEventId") WHERE "IsDeleted" = false;
CREATE INDEX "IX_PromotionItems_State" ON public."PromotionItems" ("DiscountState") WHERE "IsDeleted" = false;

-- =====================================================
-- HELPER FUNCTIONS FOR PROMOTION RESOLUTION
-- =====================================================

-- Function to get applicable promotions for a product
CREATE OR REPLACE FUNCTION get_applicable_promotions(
    p_product_id varchar(100),
    p_product_slug varchar(200),
    p_category_id varchar(100),
    p_current_date timestamptz DEFAULT NOW()
)
RETURNS TABLE (
    promotion_type varchar(20),
    promotion_id uuid,
    discount_value numeric(10,2),
    discount_type varchar(20),
    max_discount_amount numeric(10,2),
    valid_to timestamptz,
    title varchar(200),
    description text,
    priority int
) AS $$
BEGIN
    RETURN QUERY
    WITH applicable_promotions AS (
        -- Strategy 2: Item-specific promotions (Highest Priority)
        SELECT 
            'ITEM_SPECIFIC'::varchar(20) as promotion_type,
            pi."Id" as promotion_id,
            pi."DiscountValue" as discount_value,
            pi."DiscountType" as discount_type,
            pi."MaxDiscountAmount" as max_discount_amount,
            pi."ValidTo" as valid_to,
            pi."Title" as title,
            pi."Description" as description,
            1 as priority
        FROM public."PromotionItems" pi
        WHERE pi."ProductId" = p_product_id
          AND pi."DiscountState" = 'ACTIVE'
          AND pi."IsDeleted" = false
          AND (pi."ValidFrom" IS NULL OR pi."ValidFrom" <= p_current_date)
          AND (pi."ValidTo" IS NULL OR pi."ValidTo" >= p_current_date)
          AND (pi."AvailableQuantity" IS NULL OR pi."UsedQuantity" < pi."AvailableQuantity")
        
        UNION ALL
        
        -- Strategy 1: Event-based product promotions (Medium Priority)
        SELECT 
            'EVENT_PRODUCT'::varchar(20) as promotion_type,
            pg."Id" as promotion_id,
            pg."DiscountValue" as discount_value,
            pg."DiscountType" as discount_type,
            pg."MaxDiscountAmount" as max_discount_amount,
            pe."ValidTo" as valid_to,
            pg."Title" as title,
            pg."Description" as description,
            2 as priority
        FROM public."PromotionProducts" pp
        JOIN public."PromotionGlobals" pg ON pp."PromotionGlobalId" = pg."Id"
        JOIN public."PromotionEvents" pe ON pg."PromotionEventId" = pe."Id"
        WHERE pp."ProductSlug" = p_product_slug
          AND pe."DiscountState" = 'ACTIVE'
          AND pe."IsDeleted" = false
          AND pg."IsDeleted" = false
          AND pp."IsDeleted" = false
          AND (pe."ValidFrom" IS NULL OR pe."ValidFrom" <= p_current_date)
          AND (pe."ValidTo" IS NULL OR pe."ValidTo" >= p_current_date)
        
        UNION ALL
        
        -- Strategy 1: Event-based category promotions (Lower Priority)
        SELECT 
            'EVENT_CATEGORY'::varchar(20) as promotion_type,
            pg."Id" as promotion_id,
            pg."DiscountValue" as discount_value,
            pg."DiscountType" as discount_type,
            pg."MaxDiscountAmount" as max_discount_amount,
            pe."ValidTo" as valid_to,
            pg."Title" as title,
            pg."Description" as description,
            3 as priority
        FROM public."PromotionCategories" pc
        JOIN public."PromotionGlobals" pg ON pc."PromotionGlobalId" = pg."Id"
        JOIN public."PromotionEvents" pe ON pg."PromotionEventId" = pe."Id"
        WHERE pc."CategoryId" = p_category_id
          AND pe."DiscountState" = 'ACTIVE'
          AND pe."IsDeleted" = false
          AND pg."IsDeleted" = false
          AND pc."IsDeleted" = false
          AND (pe."ValidFrom" IS NULL OR pe."ValidFrom" <= p_current_date)
          AND (pe."ValidTo" IS NULL OR pe."ValidTo" >= p_current_date)
    )
    SELECT * FROM applicable_promotions
    ORDER BY priority, discount_value DESC
    LIMIT 1;
END;
$$ LANGUAGE plpgsql;

-- =====================================================
-- SAMPLE DATA FOR TESTING
-- =====================================================

-- Sample Promotion Event (Black Friday)
INSERT INTO public."PromotionEvents" ("Id", "Title", "Description", "DiscountState", "ValidFrom", "ValidTo")
VALUES (
    gen_random_uuid(),
    'Black Friday 2024',
    'Biggest sale of the year with massive discounts',
    'ACTIVE',
    '2024-11-24 00:00:00+00',
    '2024-11-26 23:59:59+00'
);

-- Sample Coupon
INSERT INTO public."Coupons" ("Id", "Code", "Title", "Description", "DiscountState", "ProductNameTag", "DiscountType", "DiscountValue", "ValidFrom", "ValidTo", "AvailableQuantity")
VALUES (
    gen_random_uuid(),
    'WELCOME20',
    'Welcome Discount',
    '20% off your first purchase',
    'ACTIVE',
    'All Products',
    'PERCENTAGE',
    20.00,
    NOW(),
    NOW() + INTERVAL '3 months',
    1000
);

-- Sample Promotion Item
INSERT INTO public."PromotionItems" ("Id", "Title", "Description", "ProductId", "ProductNameTag", "ProductImage", "ProductSlug", "DiscountType", "DiscountValue", "ValidFrom", "ValidTo", "AvailableQuantity")
VALUES (
    gen_random_uuid(),
    'iPhone 15 Pro Flash Sale',
    'Limited time offer on iPhone 15 Pro',
    'iphone-15-pro-256gb',
    'iPhone',
    'iphone-15-pro.jpg',
    'iphone-15-pro-256gb',
    'PERCENTAGE',
    15.00,
    NOW(),
    NOW() + INTERVAL '7 days',
    50
);
