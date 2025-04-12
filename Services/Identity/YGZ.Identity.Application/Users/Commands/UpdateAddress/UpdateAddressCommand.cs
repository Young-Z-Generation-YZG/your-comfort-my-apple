
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Users.Commands.UpdateAddress;

public sealed record UpdateAddressCommand : ICommand<bool>
{
}
