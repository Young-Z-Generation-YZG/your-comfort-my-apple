
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Users.Commands.AddAddress;

public sealed record AddAddressCommand() : ICommand<bool>
{
    required public string Label { get; set; }
    required public string ContactName { get; set; }
    required public string ContactPhoneNumber { get; set; }
    required public string AddressLine { get; set; }
    required public string District { get; set; }
    required public string Province { get; set; }
    required public string Country { get; set; }

}