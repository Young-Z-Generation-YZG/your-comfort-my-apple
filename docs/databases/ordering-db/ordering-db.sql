-- public."Orders" definition

-- Drop table

-- DROP TABLE public."Orders";

CREATE TABLE public."Orders" (
	"Id" uuid NOT NULL,
	"CustomerId" uuid NOT NULL,
	"Status" text DEFAULT 'PENDING'::text NOT NULL,
	"PaymentMethod" text NOT NULL,
	"SubTotalAmount" numeric NOT NULL,
	"DiscountAmount" numeric NOT NULL,
	"TotalAmount" numeric NOT NULL,
	"CreatedAt" timestamptz NOT NULL,
	"UpdatedAt" timestamptz NOT NULL,
	"LastModifiedBy" uuid NULL,
	"Code" text NOT NULL,
	"ShippingAddressAddressLine" text NOT NULL,
	"ShippingAddressContactEmail" text NOT NULL,
	"ShippingAddressContactName" text NOT NULL,
	"ShippingAddressContactPhoneNumber" text NOT NULL,
	"ShippingAddressCountry" text NOT NULL,
	"ShippingAddressDistrict" text NOT NULL,
	"ShippingAddressProvince" text NOT NULL,
	CONSTRAINT "PK_Orders" PRIMARY KEY ("Id")
);

-- public."OrderItems" definition

-- Drop table

-- DROP TABLE public."OrderItems";

CREATE TABLE public."OrderItems" (
	"Id" uuid NOT NULL,
	"ProductId" text NOT NULL,
	"ModelId" text NOT NULL,
	"ProductName" text NOT NULL,
	"ProductColorName" text NOT NULL,
	"ProductUnitPrice" numeric NOT NULL,
	"ProductImage" text NOT NULL,
	"ProductSlug" text NOT NULL,
	"Quantity" int4 NOT NULL,
	"PromotionIdOrCode" text NULL,
	"PromotionEventType" text NULL,
	"PromotionTitle" text NULL,
	"PromotionDiscountType" text NULL,
	"PromotionDiscountValue" numeric NULL,
	"PromotionDiscountUnitPrice" numeric NULL,
	"PromotionAppliedProductCount" int4 NULL,
	"PromotionFinalPrice" numeric NULL,
	"IsReviewed" bool NOT NULL,
	"OrderId" uuid NOT NULL,
	CONSTRAINT "PK_OrderItems" PRIMARY KEY ("Id"),
	CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES public."Orders"("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_OrderItems_OrderId" ON public."OrderItems" USING btree ("OrderId");