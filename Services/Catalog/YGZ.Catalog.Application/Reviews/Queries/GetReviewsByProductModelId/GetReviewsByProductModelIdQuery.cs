using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Reviews;

namespace YGZ.Catalog.Application.Reviews.Queries.GetReviewsByProductModelId;

public sealed record GetReviewsByProductModelIdQuery(string ProductModelId) : IQuery<PaginationResponse<ProductModelReviewResponse>>
{
    public int? Page { get; init; }
    public int? Limit { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
}