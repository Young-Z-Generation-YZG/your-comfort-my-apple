using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Domain.Tenants.Entities;

[BsonCollection("Branchs")]
public class Branch : Entity<BranchId>, IAuditable, ISoftDelete
{
    public Branch(BranchId id) : base(id) { }

    [BsonElement("tenant_id")]
    public required TenantId TenantId { get; init; }

    [BsonElement("name")]
    public required string Name { get; init; }

    [BsonElement("description")]
    public string? Description { get; init; }

    [BsonElement("address")]
    public required string Address { get; init; }

    [BsonElement("manager")]
    public BranchManager? Manager { get; init; }

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

    public static Branch Create(BranchId branchId, TenantId tenantId, string name, string address, BranchManager? manager, string? description = null)
    {
        return new Branch(branchId)
        {
            TenantId = tenantId,
            Name = name,
            Address = address,
            Manager = manager,
            Description = description
        };
    }

    public BranchResponse ToResponse()
    {
        return new BranchResponse
        {
            Id = Id.Value!,
            TenantId = TenantId.Value!,
            Name = Name,
            Address = Address,
            Description = Description ?? null,
            Manager = Manager?.ToResponse() ?? null,
            CreatedAt = CreatedAt.ToUniversalTime(),
            UpdatedAt = UpdatedAt.ToUniversalTime(),
            UpdatedBy = UpdatedBy ?? string.Empty,
            IsDeleted = IsDeleted,
            DeletedAt = DeletedAt?.ToUniversalTime(),
            DeletedBy = DeletedBy ?? string.Empty
        };
    }
}
