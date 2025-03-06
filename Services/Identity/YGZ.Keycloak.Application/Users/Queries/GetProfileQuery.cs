
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Keycloak.Application.Users.Queries;

public sealed record GetProfileQuery() : IQuery<string> { }