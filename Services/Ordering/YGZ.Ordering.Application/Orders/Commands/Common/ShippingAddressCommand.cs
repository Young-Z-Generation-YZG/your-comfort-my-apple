

namespace YGZ.Ordering.Application.Orders.Commands.Common;

#pragma warning disable CS8618

public sealed record ShippingAddressCommand
{
    public string ContactName { get; set; }
    public string ContactPhoneNumber { get; set; }
    public string AddressLine { get; set; }
    public string District { get; set; }
    public string Province { get; set; }
    public string Country { get; set; }
}