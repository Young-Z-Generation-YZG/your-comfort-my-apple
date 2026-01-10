using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

namespace YGZ.Catalog.Application.Inventory.Queries.GetSkuByIdWithImage;

public sealed record GetSkuByIdWithImageQuery(string SkuId) : IQuery<SkuWithImageResponse> { }
