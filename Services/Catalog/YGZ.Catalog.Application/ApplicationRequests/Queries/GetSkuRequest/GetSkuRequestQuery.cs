using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

namespace YGZ.Catalog.Application.Requests.Queries.GetSkuRequest;

public sealed record GetSkuRequestQuery(string Id) : IQuery<SkuRequestResponse>;
