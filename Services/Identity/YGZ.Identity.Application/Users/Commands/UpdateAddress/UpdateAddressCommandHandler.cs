

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Abstractions.HttpContext;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Application.Users.Commands.UpdateAddress;

public class UpdateAddressCommandHandler : ICommandHandler<UpdateAddressCommand, bool>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUserContext _userContext;

    public UpdateAddressCommandHandler(IAddressRepository addressRepository, IUserContext userContext)
    {
        _addressRepository = addressRepository;
        _userContext = userContext;
    }

    public async Task<Result<bool>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId();

        var addressId = Guid.TryParse(request.AddressId, out var guid)
            ? ShippingAddressId.Of(guid)
            : ShippingAddressId.Of(Guid.Empty);

        var addressResult = await _addressRepository.GetByIdAsync(addressId, cancellationToken);

        if (addressResult.IsFailure)
        {
            return addressResult.Error;
        }

        if(addressResult.Response!.UserId != userId)
        {
            return Errors.Address.NotFound;
        }

        addressResult.Response!.Update(
            label: request.Label,
            contactName: request.ContactName,
            contactPhoneNumber: request.ContactPhoneNumber,
            addressDetail: new Address
            {
                AddressLine = request.AddressLine,
                AddressDistrict = request.District,
                AddressProvince = request.Province,
                AddressCountry = request.Country
            },
            isDefault: false
        );

        var updateResult = await _addressRepository.UpdateAsync(addressResult.Response, cancellationToken);

        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        return true;
    }
}
