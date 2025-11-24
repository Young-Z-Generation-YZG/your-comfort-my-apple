using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;

namespace YGZ.Identity.Application.Users.Queries.GetAccount;

public sealed record GetAccountQuery : IQuery<UserResponse>
{
}
