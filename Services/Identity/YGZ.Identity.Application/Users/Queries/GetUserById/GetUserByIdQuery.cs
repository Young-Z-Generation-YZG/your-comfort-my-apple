using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;

namespace YGZ.Identity.Application.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery : IQuery<UserResponse>
{
    public required string UserId { get; init; }
}
