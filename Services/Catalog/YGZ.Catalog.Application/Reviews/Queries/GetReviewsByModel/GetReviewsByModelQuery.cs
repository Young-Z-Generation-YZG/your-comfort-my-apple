
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Reviews;

namespace YGZ.Catalog.Application.Reviews.Commands;

public sealed record GetReviewsByModelQuery(string ModelId) : IQuery<PaginationResponse<ProductReviewResponse>> 
{
    public int? Page { get; set; }
    public int? Limit { get; set; }
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; }
}