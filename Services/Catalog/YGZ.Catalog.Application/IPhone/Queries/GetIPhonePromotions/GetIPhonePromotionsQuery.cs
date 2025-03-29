using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Contracts.Common;

namespace YGZ.Catalog.Application.IPhone16.Queries.GetIPhonePromotions;

public sealed record GetIPhonePromotionsQuery() : IQuery<PaginationPromotionResponse<IPhoneResponse>> 
{
    public int? Page { get; set; }
    public int? Limit { get; set; }
    public string? IPhoneModel { get; set; }
}
