
using YGZ.Identity.Application.Users.Commands.AddAddress;
using YGZ.Identity.Domain.Users.Entities;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Application.Users.Extensions;

public static class CommandMappingExtensions
{
    public static ShippingAddress ToShippingAddress(this AddAddressCommand request, string userId)
    {
        return ShippingAddress.Create(id: ShippingAddressId.Create(),
                                      label: request.Label,
                                      contactName: request.ContactName,
                                      contactPhoneNumber: request.ContactPhoneNumber,
                                      addressDetail: Address.Create(
                                          addressLine: request.AddressLine,
                                          addressDistrict: request.District,
                                          addressProvince: request.Province,
                                          addressCountry: request.Country),
                                      isDefault: false,
                                      userId: userId);
    }
}
