
using MediatR;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Application.Users.Commands.DeleteAddress;

public class DeleteAddressCommandHandler : ICommandHandler<DeleteAddressCommand, bool>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUserRepository _userRepository;
    public DeleteAddressCommandHandler(IAddressRepository addressRepository, IUserRepository userRepository)
    {
        _addressRepository = addressRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<bool>> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        //var userEmail = _userContext.GetUserEmail();
        var userEmail = "lov3rinve146@gmail.com";

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

        var result = await _addressRepository.RemoveAsync(addressResult.Response!, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return true;
    }
}
