using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Requests.SkuRequest.Events;
using YGZ.Catalog.Domain.Requests.SkuRequest.ValueObjects;
using YGZ.Catalog.Domain.Tenants.ValueObjects;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

namespace YGZ.Catalog.Domain.Requests.SkuRequest;

[BsonCollection("SkuRequests")]
public class SkuRequest : AggregateRoot<RequestId>, IAuditable, ISoftDelete
{
    public SkuRequest(RequestId id) : base(id)
    {
    }

    [BsonElement("sender_user_id")]
    public required string SenderUserId { get; init; }

    [BsonElement("from_branch")]
    public required EmbeddedBranch FromBranch { get; init; }

    [BsonElement("to_branch")]
    public required EmbeddedBranch ToBranch { get; init; }

    [BsonElement("sku")]
    public required EmbeddedSku Sku { get; init; }

    [BsonElement("request_quantity")]
    public int RequestQuantity { get; private set; }

    [BsonElement("state")]
    public string State { get; private set; } = ESkuRequestState.PENDING.Name;

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_by")]
    public string? UpdatedBy { get; set; }

    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [BsonElement("deleted_by")]
    public string? DeletedBy { get; set; }

    public static SkuRequest Create(RequestId requestId, string senderUserId, EmbeddedBranch fromBranch, EmbeddedBranch toBranch, EmbeddedSku sku, int requestQuantity, ESkuRequestState state)
    {
        if (requestQuantity <= 0)
        {
            throw new ArgumentException("Request quantity must be greater than 0");
        }

        return new SkuRequest(requestId)
        {
            SenderUserId = senderUserId,
            FromBranch = fromBranch,
            ToBranch = toBranch,
            Sku = sku,
            RequestQuantity = requestQuantity,
            State = state.Name
        };
    }

    public void Approve()
    {
        if (State != ESkuRequestState.PENDING.Name)
        {
            throw new InvalidOperationException("Only pending requests can be approved.");
        }
        State = ESkuRequestState.APPROVED.Name;
        UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new SkuRequestApprovedDomainEvent(this));
    }

    public void Reject()
    {
        if (State != ESkuRequestState.PENDING.Name)
        {
            throw new InvalidOperationException("Only pending requests can be rejected.");
        }
        State = ESkuRequestState.REJECTED.Name;
        UpdatedAt = DateTime.UtcNow;
    }

    public SkuRequestResponse ToResponse()
    {
        return new SkuRequestResponse
        {
            Id = Id.Value!,
            SenderUserId = SenderUserId,
            FromBranch = FromBranch.ToResponse(),
            ToBranch = ToBranch.ToResponse(),
            Sku = Sku.ToResponse(),
            RequestQuantity = RequestQuantity,
            State = State,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            UpdatedBy = UpdatedBy,
            IsDeleted = IsDeleted,
            DeletedAt = DeletedAt,
            DeletedBy = DeletedBy
        };
    }
}
