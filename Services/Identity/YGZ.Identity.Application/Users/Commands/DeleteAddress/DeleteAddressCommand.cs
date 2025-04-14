
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Users.Commands.DeleteAddress;

public sealed record DeleteAddressCommand(string AddressId) : ICommand<bool> { }
