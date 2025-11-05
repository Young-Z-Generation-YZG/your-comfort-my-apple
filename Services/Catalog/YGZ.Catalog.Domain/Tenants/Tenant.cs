using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Tenants;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.Events;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Domain.Tenants;

[BsonCollection("Tenants")]
public class Tenant : AggregateRoot<TenantId>, IAuditable, ISoftDelete
{
    public Tenant(TenantId id) : base(id) { }

    [BsonElement("name")]
    public required string Name { get; init; }

    [BsonElement("sub_domain")]
    public required string SubDomain { get; init; }

    [BsonElement("description")]
    public string? Description { get; init; }

    [BsonElement("tenant_type")]
    public required string TenantType { get; init; }

    [BsonElement("tenant_state")]
    public required string TenantState { get; init; }

    [BsonElement("embedded_branch")]
    public Branch? EmbeddedBranch { get; set; } = null;

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

    public static Tenant Create(TenantId tenantId, string name, string subDomain, ETenantType tenantType, Branch branch, string? description = null)
    {
        var tenant = new Tenant(tenantId)
        {
            Name = name,
            SubDomain = subDomain,
            Description = description,
            TenantType = tenantType.Name,
            TenantState = ETenantState.ACTIVE.Name,
            EmbeddedBranch = branch
        };

        tenant.AddDomainEvent(new TenantCreatedDomainEvent(Tenant: tenant,
                                                           Branch: branch));

        return tenant;
    }

    public void SetEmbeddedBranch(Branch branch)
    {
        EmbeddedBranch = branch;
    }

    public TenantResponse ToResponse()
    {
        return new TenantResponse
        {
            Id = Id?.Value?.ToString() ?? string.Empty,
            SubDomain = SubDomain,
            Name = Name,
            Description = Description ?? string.Empty,
            TenantType = TenantType,
            TenantState = TenantState,
            EmbeddedBranch = EmbeddedBranch?.ToResponse() ?? null,
            CreatedAt = CreatedAt.ToUniversalTime(),
            UpdatedAt = UpdatedAt.ToUniversalTime(),
            UpdatedBy = UpdatedBy ?? string.Empty,
            IsDeleted = IsDeleted,
            DeletedAt = DeletedAt?.ToUniversalTime(),
            DeletedBy = DeletedBy ?? string.Empty
        };
    }
}
