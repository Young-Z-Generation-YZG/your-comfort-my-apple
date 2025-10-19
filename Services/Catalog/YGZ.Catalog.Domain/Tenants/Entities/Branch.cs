using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;
using YGZ.Catalog.Domain.Core.Abstractions;
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

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;

    public string? UpdatedBy { get; init; } = null;

    public bool IsDeleted { get; init; } = false;

    public DateTime? DeletedAt { get; init; } = null;

    public string? DeletedBy { get; init; } = null;

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
            Manager = Manager?.ToResponse() ?? null
        };
    }
}
