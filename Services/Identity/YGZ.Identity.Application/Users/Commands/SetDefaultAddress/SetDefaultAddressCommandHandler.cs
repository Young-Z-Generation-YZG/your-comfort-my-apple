

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Abstractions.HttpContext;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Application.Users.Commands.SetDefaultAddress;

public class SetDefaultAddressCommandHandler : ICommandHandler<SetDefaultAddressCommand, bool>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;

    public SetDefaultAddressCommandHandler(IAddressRepository addressRepository, IUserRepository userRepository, IUserContext userContext)
    {
        _addressRepository = addressRepository;
        _userRepository = userRepository;
        _userContext = userContext;
    }

    public async Task<Result<bool>> Handle(SetDefaultAddressCommand request, CancellationToken cancellationToken)
    {
        var addressId = Guid.TryParse(request.AddressId, out var guid)
            ? ShippingAddressId.Of(guid)
            : ShippingAddressId.Of(Guid.Empty);

        //var userEmail = _userContext.GetUserEmail();
        var userEmail = "lov3rinve146@gmail.com";

        var userResult = await _userRepository.GetUserByEmailAsync(userEmail, cancellationToken);

        if (userResult.IsFailure)
        {
            return userResult.Error;
        }

        var addressResult = await _addressRepository.GetByIdAsync(addressId, cancellationToken);

        if (addressResult.IsFailure)
        {
            return addressResult.Error;
        }

        var result = await _addressRepository.SetDefaultAddressAsync(userResult.Response!, addressResult.Response!, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return true;
    }
}
