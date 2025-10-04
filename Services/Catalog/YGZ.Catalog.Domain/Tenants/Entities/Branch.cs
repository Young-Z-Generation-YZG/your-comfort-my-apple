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

    [BsonElement("created_at")]
    public DateTime CreatedAt => DateTime.Now;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt => DateTime.Now;

    [BsonElement("modified_by")]
    public string? ModifiedBy => null;

    [BsonElement("is_deleted")]
    public bool IsDeleted => false;

    [BsonElement("deleted_at")]
    public DateTime? DeletedAt => null;

    [BsonElement("deleted_by")]
    public string? DeletedBy => null;

    public static Branch Create(TenantId tenantId, string name, string address, BranchManager? manager, string? description = null)
    {
        return new Branch(BranchId.Create())
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
