namespace YGZ.Catalog.Api.Contracts.ReviewRequest;

public class GetReviewsByModelRequest
{
    public int? _page { get; set; } = 1;
    public int? _limit { get; set; } = 10;
    public string? _sortBy { get; set; }
    public string? _sortOrder { get; set; } = "desc";
}
