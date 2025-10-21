using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Tenants;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.Events;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Domain.Tenants;

[BsonCollection("Tenants")]
public class Tenant : AggregateRoot<TenantId>, IAuditable, ISoftDelete
{
    public Tenant(TenantId id) : base(id) { }

    [BsonElement("code")]
    public required string Code { get; init; }

    [BsonElement("name")]
    public required string Name { get; init; }

    [BsonElement("description")]
    public string? Description { get; init; }

    [BsonElement("tenant_type")]
    public required string TenantType { get; init; }

    [BsonElement("tenant_state")]
    public required string TenantState { get; init; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;

    public string? UpdatedBy { get; init; } = null;

    public bool IsDeleted { get; init; } = false;

    public DateTime? DeletedAt { get; init; } = null;

    public string? DeletedBy { get; init; } = null;

    public static Tenant Create(TenantId tenantId, string name, ETenantType tenantType, Branch branch, string? description = null)
    {
        var tenant = new Tenant(tenantId)
        {
            Code = SnakeCaseSerializer.Serialize(name).ToUpper(),
            Name = name,
            Description = description,
            TenantType = tenantType.Name,
            TenantState = ETenantState.ACTIVE.Name
        };

        tenant.AddDomainEvent(new TenantCreatedDomainEvent(Tenant: tenant,
                                                           Branch: branch));

        return tenant;
    }

    public TenantResponse ToResponse()
    {
        return new TenantResponse
        {
            Id = Id?.Value?.ToString() ?? string.Empty,
            Code = Code,
            Name = Name,
            Description = Description ?? string.Empty,
            TenantType = TenantType,
            TenantState = TenantState,
            CreatedAt = CreatedAt.ToUniversalTime(),
            UpdatedAt = UpdatedAt.ToUniversalTime(),
            UpdatedBy = UpdatedBy ?? string.Empty,
            IsDeleted = IsDeleted,
            DeletedAt = DeletedAt?.ToUniversalTime(),
            DeletedBy = DeletedBy ?? string.Empty
        };
    }
}
