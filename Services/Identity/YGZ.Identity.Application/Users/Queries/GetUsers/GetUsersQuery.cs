using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;

namespace YGZ.Identity.Application.Users.Queries.GetUsers;

public sealed record GetUsersQuery : IQuery<PaginationResponse<UserResponse>>
{
    public int? Page { get; init; }
    public int? Limit { get; init; }
    public string? Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? PhoneNumber { get; init; }
}
