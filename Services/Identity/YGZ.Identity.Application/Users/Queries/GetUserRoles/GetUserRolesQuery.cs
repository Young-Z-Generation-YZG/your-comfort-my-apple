using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Users.Queries.GetUserRoles;

public sealed record GetUserRolesQuery : IQuery<List<string>>
{
    public required string UserId { get; init; }
}


