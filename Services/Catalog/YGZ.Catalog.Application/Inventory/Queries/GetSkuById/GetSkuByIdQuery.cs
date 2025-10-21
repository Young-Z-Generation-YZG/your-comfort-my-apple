using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;

namespace YGZ.Catalog.Application.Inventory.Queries.GetSkuById;

public sealed record GetSkuByIdQuery(string SkuId) : IQuery<SkuResponse> { }
