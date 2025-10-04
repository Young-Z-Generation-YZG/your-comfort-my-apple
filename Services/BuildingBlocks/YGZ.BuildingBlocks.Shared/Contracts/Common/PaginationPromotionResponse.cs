

using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Common;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public class PaginationPromotionResponse<TData>
{
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public IEnumerable<TData> Items { get; set; } = new List<TData>();
    //public IEnumerable<PromotionDataResponse> PromotionItems { get; set; } = new List<PromotionDataResponse>();
    public PaginationLinks Links { get; set; } = new PaginationLinks("", "", "", "");
}
