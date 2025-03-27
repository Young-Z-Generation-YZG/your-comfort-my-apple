
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.IPhone16.Queries.GetIPhone16Models;

public sealed record GetIPhone16ModelsQuery() : IQuery<bool> { }