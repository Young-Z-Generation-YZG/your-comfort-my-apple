using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;
using YGZ.BuildingBlocks.Shared.Enums;
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

    [BsonElement("product_classification")]
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
    public int AvailableInStock { get; private set; } = 0;

    [BsonElement("reserved_for_event")]
    public ReservedForEvent? ReservedForEvent { get; private set; }

    [BsonElement("total_sold")]
    public int TotalSold { get; set; } = 0;

    [BsonElement("state")]
    public string State { get; private set; }

    [BsonElement("slug")]
    public Slug Slug { get; set; } = default!;

    [BsonElement("CreatedAt")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [BsonElement("UpdatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("UpdatedBy")]
    public string? UpdatedBy { get; set; } = null;

    [BsonElement("IsDeleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("DeletedAt")]
    public DateTime? DeletedAt { get; set; } = null;

    [BsonElement("DeletedBy")]
    public string? DeletedBy { get; set; } = null;

    public static SKU Create(
        SkuId skuId,
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
        var sku = new SKU(skuId)
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

    public void SetReservedForEvent(string eventId, string eventItemId, string eventName, int reservedQuantity)
    {
        if (ReservedForEvent is not null && ReservedForEvent.EventItemId == eventItemId)
        {
            AvailableInStock -= reservedQuantity;

            ReservedForEvent.ReservedQuantity += reservedQuantity;
        }
        else
        {
            AvailableInStock -= reservedQuantity;

            ReservedForEvent = ReservedForEvent.Create(eventId, eventItemId, eventName, reservedQuantity);
        }
    }

    public void DeductQuantity(int quantity)
    {
        if (AvailableInStock < quantity)
        {
            throw new Exception("Reserved quantity is less than the quantity to deduct");
        }

        AvailableInStock -= quantity;
    }

    public void DeductReservedQuantity(int quantity)
    {
        if (ReservedForEvent is not null)
        {
            if (ReservedForEvent.ReservedQuantity < quantity)
            {
                throw new Exception("Reserved quantity is less than the quantity to deduct");
            }

            ReservedForEvent.ReservedQuantity -= quantity;
        }
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
            UpdatedBy = UpdatedBy,
            DeletedAt = DeletedAt,
            DeletedBy = DeletedBy,
            IsDeleted = IsDeleted
        };
    }
}
