using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Auths.Commands.AddNewStaff;

public sealed record AddNewStaffCommand : ICommand<bool>
{
    public string? TenantId { get; init; }
    public string? BranchId { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string PhoneNumber { get; init; }
    public required string RoleName { get; init; }
}
