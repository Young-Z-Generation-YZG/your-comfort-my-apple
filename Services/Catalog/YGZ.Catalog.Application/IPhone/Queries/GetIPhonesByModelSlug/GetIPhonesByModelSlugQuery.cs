

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

namespace YGZ.Catalog.Application.IPhone16.Queries.GetIPhonesByModelSlug;

public sealed record GetIPhonesByModelSlugQuery(string modelSlug) : IQuery<List<IPhoneResponse>>;