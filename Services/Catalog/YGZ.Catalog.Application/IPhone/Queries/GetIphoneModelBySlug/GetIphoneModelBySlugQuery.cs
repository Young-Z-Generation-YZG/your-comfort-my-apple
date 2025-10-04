using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;

namespace YGZ.Catalog.Application.Iphone.Queries.GetIphoneModelBySlug;

public sealed record GetIphoneModelBySlugQuery(string ModelSlug) : IQuery<IphoneModelDetailsResponse>;
