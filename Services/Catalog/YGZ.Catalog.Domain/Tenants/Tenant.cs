using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Domain.Tenants;

[BsonCollection("Tenants")]
public class Tenant : AggregateRoot<TenantId>, IAuditable, ISoftDelete
{
    public Tenant(TenantId id) : base(id) { }

    [BsonElement("code")]
    public required string Code;

    [BsonElement("name")]
    public required string Name;

    [BsonElement("description")]
    public string? Description;

    [BsonElement("state")]
    public required string TenantState;

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

    public static Tenant Create(string code, string name, string? description = null)
    {
        var tenant = new Tenant(TenantId.Create())
        {
            Code = code,
            Name = name,
            Description = description,
            TenantState = ETenantState.ACTIVE.Name
        };

        return tenant;
    }
}
