using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Users.Queries.GetProfile;

public sealed record GetProfileQuery() : IQuery<string> { }