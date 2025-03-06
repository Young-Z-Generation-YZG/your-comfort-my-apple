

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Keycloak.Application.Users.Commands;

public sealed record UpdateProfileCommand() : ICommand<bool> { }
