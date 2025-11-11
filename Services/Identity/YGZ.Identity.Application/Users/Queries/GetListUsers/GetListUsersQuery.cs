using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;

namespace YGZ.Identity.Application.Users.Queries.GetListUsers;

public sealed record GetListUsersQuery : IQuery<List<UserResponse>>
{
    public List<string>? Roles { get; init; }
}
