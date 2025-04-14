
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Users.Commands.UpdateAddress;

public sealed record UpdateAddressCommand : ICommand<bool>
{
    required public string AddressId { get; set; }
    required public string Label { get; init; }
    required public string ContactName { get; init; }
    required public string ContactPhoneNumber { get; init; }
    required public string AddressLine { get; init; }
    required public string District { get; init; }
    required public string Province { get; init; }
    required public string Country { get; init; }
}