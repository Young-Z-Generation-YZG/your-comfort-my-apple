

using System.Windows.Input;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Users.Commands.SetDefaultAddress;

public sealed record SetDefaultAddressCommand(string AddressId) : ICommand<bool> { }
