using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Domain.Requests.SkuRequest.ValueObjects;

public class EmbeddedBranch : ValueObject
{
    [BsonElement("branch_id")]
    public BranchId BranchId { get; init; }

    [BsonElement("branch_name")]
    public string BranchName { get; init; }

    private EmbeddedBranch(BranchId branchId, string branchName)
    {
        BranchId = branchId;
        BranchName = branchName;
    }

    public static EmbeddedBranch Create(BranchId branchId, string branchName)
    {
        return new EmbeddedBranch(branchId, branchName);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return BranchId;
        yield return BranchName;
    }

    public EmbeddedBranchResponse ToResponse()
    {
        return new EmbeddedBranchResponse
        {
            BranchId = BranchId.Value!,
            BranchName = BranchName,
        };
    }
}