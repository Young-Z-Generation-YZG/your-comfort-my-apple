using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class ReservedForSkuRequest : ValueObject
{
    [BsonElement("to_branch_id")]
    public BranchId ToBranchId { get; init; }

    [BsonElement("to_branch_name")]
    public string ToBranchName { get; init; }

    [BsonElement("reserved_quantity")]
    public int ReservedQuantity { get; set; }


    private ReservedForSkuRequest(BranchId branchId, string branchName, int reservedQuantity)
    {
        ToBranchId = branchId;
        ToBranchName = branchName;
        ReservedQuantity = reservedQuantity;
    }

    public static ReservedForSkuRequest Create(BranchId branchId, string branchName, int reservedQuantity)
    {
        return new ReservedForSkuRequest(branchId, branchName, reservedQuantity);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return ToBranchId;
        yield return ToBranchName;
        yield return ReservedQuantity;
    }

    public ReservedForSkuRequestResponse ToResponse()
    {
        return new ReservedForSkuRequestResponse
        {
            ToBranchId = ToBranchId.Value!,
            ToBranchName = ToBranchName,
            ReservedQuantity = ReservedQuantity
        };
    }
}