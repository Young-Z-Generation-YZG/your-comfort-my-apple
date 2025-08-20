-- public."Coupons" definition
CREATE TABLE public."Coupons" (
	"Id" uuid NOT NULL,
	"Code" text NOT NULL,
	"Title" text NOT NULL,
	"Description" text NOT NULL,
	"DiscountState" text NOT NULL,
	"ProductNameTag" text NOT NULL,
	"PromotionEventType" text NOT NULL,
	"DiscountType" text NOT NULL,
	"DiscountValue" numeric NOT NULL,
	"MaxDiscountAmount" numeric NULL,
	"ValidFrom" timestamptz NULL,
	"ValidTo" timestamptz NULL,
	"AvailableQuantity" int4 NOT NULL,
	"CreatedAt" timestamptz NOT NULL,
	"UpdatedAt" timestamptz NOT NULL,
	"IsDeleted" bool NOT NULL,
	"DeletedAt" timestamptz NULL,
	"DeletedBy" text NULL,
	CONSTRAINT "PK_Coupons" PRIMARY KEY ("Id")
);


-- public."PromotionEvents" definition
CREATE TABLE public."PromotionEvents" (
	"Id" uuid NOT NULL,
	"Title" text NOT NULL,
	"Description" text NOT NULL,
	"PromotionEventType" text NOT NULL,
	"DiscountState" text NOT NULL,
	"ValidFrom" timestamptz NULL,
	"ValidTo" timestamptz NULL,
	"CreatedAt" timestamptz NOT NULL,
	"UpdatedAt" timestamptz NOT NULL,
	"IsDeleted" bool NOT NULL,
	"DeletedAt" timestamptz NULL,
	"DeletedBy" text NULL,
	CONSTRAINT "PK_PromotionEvents" PRIMARY KEY ("Id")
);


-- public."PromotionItems" definition
CREATE TABLE public."PromotionItems" (
	"Id" uuid NOT NULL,
	"Title" text NOT NULL,
	"Description" text NOT NULL,
	"ProductNameTag" text NOT NULL,
	"PromotionEventType" text NOT NULL,
	"DiscountState" text NOT NULL,
	"DiscountType" text NOT NULL,
	"EndDiscountType" text NOT NULL,
	"DiscountValue" numeric NOT NULL,
	"ValidFrom" timestamptz NULL,
	"ValidTo" timestamptz NULL,
	"AvailableQuantity" int4 NULL,
	"ProductId" text NOT NULL,
	"ProductImage" text NOT NULL,
	"ProductSlug" text NOT NULL,
	"CreatedAt" timestamptz NOT NULL,
	"UpdatedAt" timestamptz NOT NULL,
	"IsDeleted" bool NOT NULL,
	"DeletedAt" timestamptz NULL,
	"DeletedBy" text NULL,
	CONSTRAINT "PK_PromotionItems" PRIMARY KEY ("Id")
);


-- public."PromotionGlobals" definition
CREATE TABLE public."PromotionGlobals" (
	"Id" uuid NOT NULL,
	"Title" text NOT NULL,
	"Description" text NOT NULL,
	"PromotionGlobalType" text NOT NULL,
	"PromotionEventId" uuid NOT NULL,
	"CreatedAt" timestamptz NOT NULL,
	"UpdatedAt" timestamptz NOT NULL,
	"IsDeleted" bool NOT NULL,
	"DeletedAt" timestamptz NULL,
	"DeletedBy" text NULL,
	CONSTRAINT "PK_PromotionGlobals" PRIMARY KEY ("Id"),
	CONSTRAINT "FK_PromotionGlobals_PromotionEvents_PromotionEventId" FOREIGN KEY ("PromotionEventId") REFERENCES public."PromotionEvents"("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_PromotionGlobals_PromotionEventId" ON public."PromotionGlobals" USING btree ("PromotionEventId");


-- public."PromotionProducts" definition
CREATE TABLE public."PromotionProducts" (
	"Id" text NOT NULL,
	"ProductSlug" text NOT NULL,
	"ProductImage" text NOT NULL,
	"DiscountType" text NOT NULL,
	"DiscountValue" numeric NOT NULL,
	"PromotionGlobalId" uuid NOT NULL,
	CONSTRAINT "PK_PromotionProducts" PRIMARY KEY ("Id"),
	CONSTRAINT "FK_PromotionProducts_PromotionGlobals_PromotionGlobalId" FOREIGN KEY ("PromotionGlobalId") REFERENCES public."PromotionGlobals"("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_PromotionProducts_PromotionGlobalId" ON public."PromotionProducts" USING btree ("PromotionGlobalId");


-- public."PromotionCategories" definition
CREATE TABLE public."PromotionCategories" (
	"Id" text NOT NULL,
	"CategoryName" text NOT NULL,
	"CategorySlug" text NOT NULL,
	"DiscountType" text NOT NULL,
	"DiscountValue" numeric NOT NULL,
	"PromotionGlobalId" uuid NOT NULL,
	CONSTRAINT "PK_PromotionCategories" PRIMARY KEY ("Id"),
	CONSTRAINT "FK_PromotionCategories_PromotionGlobals_PromotionGlobalId" FOREIGN KEY ("PromotionGlobalId") REFERENCES public."PromotionGlobals"("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_PromotionCategories_PromotionGlobalId" ON public."PromotionCategories" USING btree ("PromotionGlobalId");