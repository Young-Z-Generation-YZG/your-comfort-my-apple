
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Abstractions.HttpContext;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Application.Users.Commands.DeleteAddress;

public class DeleteAddressHandler : ICommandHandler<DeleteAddressCommand, bool>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserRequestContext _userContext;
    public DeleteAddressHandler(IAddressRepository addressRepository, IUserRepository userRepository, IUserRequestContext userContext)
    {
        _addressRepository = addressRepository;
        _userRepository = userRepository;
        _userContext = userContext;
    }

    public async Task<Result<bool>> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var userEmail = _userContext.GetUserEmail();
        var userId = _userContext.GetUserId();

        var addressId = Guid.TryParse(request.AddressId, out var guid)
            ? ShippingAddressId.Of(guid)
            : ShippingAddressId.Of(Guid.Empty);

        var userResult = await _userRepository.GetUserByEmailAsync(userEmail, cancellationToken);

        if (userResult.IsFailure)
        {
            return false;
        }

        var addressResult = await _addressRepository.GetByIdAsync(addressId, cancellationToken);

        if (addressResult.IsFailure)
        {
            return addressResult.Error;
        }

        if (addressResult.Response!.UserId != userId)
        {
            return Errors.Address.NotFound;
        }

        var result = await _addressRepository.RemoveAsync(addressResult.Response!, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return true;
    }
}
