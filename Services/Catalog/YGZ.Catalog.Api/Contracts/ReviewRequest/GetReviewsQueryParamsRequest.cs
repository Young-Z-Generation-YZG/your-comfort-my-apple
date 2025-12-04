namespace YGZ.Catalog.Api.Contracts.ReviewRequest;

public class GetReviewsQueryParamsRequest
{
    public int? _page { get; init; }
    public int? _limit { get; init; }
    public string? _sortBy { get; init; }
    public string? _sortOrder { get; init; }
}
