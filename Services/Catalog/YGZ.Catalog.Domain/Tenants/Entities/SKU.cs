using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Domain.Tenants.Entities;

[BsonCollection("SKUs")]
public class SKU : Entity<SkuId>, IAuditable, ISoftDelete
{
    public SKU(SkuId id) : base(id) { }

    [BsonElement("model_id")]
    public required ModelId ModelId { get; init; }

    [BsonElement("tenant_id")]
    public required TenantId TenantId { get; init; }

    [BsonElement("branch_id")]
    public required BranchId BranchId { get; init; }

    [BsonElement("sku_code")]
    public required SkuCode SkuCode { get; init; }

    [BsonElement("product_type")]
    public string ProductClassification { get; private set; }

    [BsonElement("model")]
    public required Model Model { get; init; }

    [BsonElement("color")]
    public required Color Color { get; init; }

    [BsonElement("storage")]
    public required Storage Storage { get; init; }

    [BsonElement("unit_price")]
    public decimal UnitPrice { get; set; } = 0;

    [BsonElement("available_in_stock")]
    public int AvailableInStock { get; set; } = 0;

    [BsonElement("reserved_for_event")]
    public ReservedForEvent? ReservedForEvent { get; init; }

    [BsonElement("total_sold")]
    public int TotalSold { get; set; } = 0;

    [BsonElement("state")]
    public string State { get; private set; }

    [BsonElement("slug")]
    public Slug Slug { get; set; } = default!;

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;

    public string? UpdatedBy { get; init; } = null;

    public bool IsDeleted { get; init; } = false;

    public DateTime? DeletedAt { get; init; } = null;

    public string? DeletedBy { get; init; } = null;

    public static SKU Create(
        ModelId modelId,
        TenantId tenantId,
        BranchId branchId,
        SkuCode skuCode,
        EProductClassification productClassification,
        Model model,
        Color color,
        Storage storage,
        decimal unitPrice,
        int availableInStock = 0)
    {
        var sku = new SKU(SkuId.Create())
        {
            ModelId = modelId,
            TenantId = tenantId,
            BranchId = branchId,
            SkuCode = skuCode,
            ProductClassification = productClassification.Name,
            Model = model,
            Color = color,
            Storage = storage,
            UnitPrice = unitPrice,
            AvailableInStock = availableInStock,
            ReservedForEvent = null,
            State = EProductState.ACTIVE.Name,
            Slug = Slug.Create(skuCode.Value)
        };

        return sku;
    }

    public void SetType(EProductClassification productClassification)
    {
        ProductClassification = productClassification.Name;
    }

    public void SetState(EProductState state)
    {
        State = state.Name;
    }

    public SkuResponse ToResponse()
    {
        return new SkuResponse
        {
            Id = Id.Value!,
            Code = SkuCode.Value,
            ModelId = ModelId.Value!,
            TenantId = TenantId.Value!,
            BranchId = BranchId.Value!,
            ProductClassification = ProductClassification,
            Model = Model.ToResponse(),
            Color = Color.ToResponse(),
            Storage = Storage.ToResponse(),
            UnitPrice = UnitPrice,
            AvailableInStock = AvailableInStock,
            TotalSold = TotalSold,
            ReservedForEvent = ReservedForEvent?.ToResponse(),
            State = State,
            Slug = Slug.Value,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            DeletedAt = DeletedAt,
            DeletedBy = DeletedBy,
            IsDeleted = IsDeleted
        };
    }
}
