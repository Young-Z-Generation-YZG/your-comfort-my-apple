using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Users.Commands.UpdateProfile;

public sealed record UpdateProfileCommand() : ICommand<bool> { }
