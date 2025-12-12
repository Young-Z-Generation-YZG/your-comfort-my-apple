using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Users.Commands.AssignRoles;

public sealed record AssignRolesCommand : ICommand<bool>
{
    public required string UserId { get; set; }
    public required List<string> Roles { get; init; }
}

