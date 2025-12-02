using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Auths.Commands.AssignRoles;

public sealed record AssignRolesCommand : ICommand<bool>
{
    public required string UserId { get; init; }
    public required List<string> Roles { get; init; }
}
