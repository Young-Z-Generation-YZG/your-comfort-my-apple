
using YGZ.Identity.Domain.Users.Entities;
using YGZ.Identity.Domain.Users.Events;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Application.Users.Extensions;

public static class DomainMappingExtensions
{
    public static ShippingAddress ToShippingAddress(this UserCreatedDomainEvent request)
    {
        var address = Address.Create(
            addressLine: "",
            addressDistrict: "",
            addressProvince: "",
            addressCountry: request.Country);

        return ShippingAddress.Create(id: ShippingAddressId.Create(),
                                      contactName: $"{request.FirstName} {request.LastName}",
                                      contactPhoneNumber: request.User.PhoneNumber!,
                                      addressDetail: address,
                                      isDefault: true,
                                      userId: request.User.Id);
    }

    public static Profile ToProfile(this UserCreatedDomainEvent request) { 

        return Profile.Create(id: ProfileId.Create(),
                              firstName: request.FirstName,
                              lastName: request.LastName,
                              birthDay: request.BirthDay,
                              gender: request.Gender,
                              image: null,
                              userId: request.User.Id);
    }
}
