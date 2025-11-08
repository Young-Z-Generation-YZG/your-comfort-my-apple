
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Reviews;

namespace YGZ.Catalog.Application.Reviews.Queries.GetReviewsByModel;

public sealed record GetReviewsByProductModelSlugQuery(string ProductModelSlug) : IQuery<PaginationResponse<ProductModelReviewResponse>> 
{
    public int? Page { get; init; }
    public int? Limit { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
}