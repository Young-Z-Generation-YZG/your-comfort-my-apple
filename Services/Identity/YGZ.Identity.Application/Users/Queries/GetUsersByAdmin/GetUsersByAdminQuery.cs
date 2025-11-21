using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;

namespace YGZ.Identity.Application.Users.Queries.GetUsersByAdmin;

public sealed record GetUsersByAdminQuery : IQuery<PaginationResponse<UserResponse>>
{
    public string? TenantId { get; init; }
    public int? Page { get; init; }
    public int? Limit { get; init; }
    public string? Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? PhoneNumber { get; init; }
}
