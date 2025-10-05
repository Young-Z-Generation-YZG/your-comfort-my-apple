using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Domain.Tenants.Entities;

[BsonCollection("SKUs")]
public class SKU : Entity<SKUId>, IAuditable, ISoftDelete
{
    public SKU(SKUId id) : base(id) { }

    [BsonElement("model_id")]
    public required ModelId ModelId { get; init; }

    [BsonElement("tenant_id")]
    public required TenantId TenantId { get; init; }

    [BsonElement("branch_id")]
    public required BranchId BranchId { get; init; }

    [BsonElement("sku_code")]
    public required SKUCode SKUCode { get; init; }

    [BsonElement("product_type")]
    public string ProductType { get; private set; }

    [BsonElement("model")]
    public Model Model { get; set; } = default!;

    [BsonElement("color")]
    public Color Color { get; set; } = default!;

    [BsonElement("storage")]
    public Storage Storage { get; set; } = default!;

    [BsonElement("unit_price")]
    public decimal UnitPrice { get; set; } = 0;

    [BsonElement("available_in_stock")]
    public int AvailableInStock { get; set; } = 0;

    [BsonElement("total_sold")]
    public int TotalSold { get; set; } = 0;

    [BsonElement("state")]
    public string State { get; private set; }

    [BsonElement("slug")]
    public Slug Slug { get; set; } = default!;

    public DateTime CreatedAt => DateTime.UtcNow;

    public DateTime UpdatedAt => DateTime.UtcNow;

    public string? ModifiedBy => null;

    public bool IsDeleted => false;

    public DateTime? DeletedAt => null;

    public string? DeletedBy => null;

    public static SKU Create(
        ModelId modelId,
        TenantId tenantId,
        BranchId branchId,
        SKUCode skuCode,
        EProductType productType,
        Model model,
        Color color,
        Storage storage,
        decimal unitPrice,
        int availableInStock = 0)
    {
        var sku = new SKU(SKUId.Create())
        {
            ModelId = modelId,
            TenantId = tenantId,
            BranchId = branchId,
            SKUCode = skuCode,
            ProductType = productType.Name,
            Model = model,
            Color = color,
            Storage = storage,
            UnitPrice = unitPrice,
            AvailableInStock = availableInStock,
            State = EState.ACTIVE.Name,
            Slug = Slug.Create(skuCode.Value)
        };

        return sku;
    }

    public void SetType(EProductType productType)
    {
        ProductType = productType.Name;
    }

    public void SetState(EState state)
    {
        State = state.Name;
    }

    public SkuResponse ToResponse()
    {
        return new SkuResponse
        {
            Id = Id.Value!,
            Code = SKUCode.Value,
            ModelId = ModelId.Value!,
            TenantId = TenantId.Value!,
            BranchId = BranchId.Value!,
            ProductType = ProductType,
            Model = Model.ToResponse(),
            Color = Color.ToResponse(),
            Storage = Storage.ToResponse(),
            UnitPrice = UnitPrice,
            AvailableInStock = AvailableInStock,
            TotalSold = TotalSold,
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
