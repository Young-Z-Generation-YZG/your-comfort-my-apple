
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

namespace YGZ.Catalog.Application.IPhone16.Queries.GetIPhone16Models;

public sealed record GetModelsBySlugQuery(string ModelSlug) : IQuery<IPhoneModelResponse> { }