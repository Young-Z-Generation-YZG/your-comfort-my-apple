

using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Users.Entities;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Application.Users.Commands.SetDefaultAddress;

public class SetDefaultAddressHandler : ICommandHandler<SetDefaultAddressCommand, bool>
{
    private readonly IUserHttpContext _userHttpContext;
    private readonly ILogger<SetDefaultAddressHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IGenericRepository<ShippingAddress, ShippingAddressId> _addressRepository;

    public SetDefaultAddressHandler(IUserHttpContext userHttpContext,
                                ILogger<SetDefaultAddressHandler> logger,
                                IUserRepository userRepository,
                                IGenericRepository<ShippingAddress, ShippingAddressId> addressRepository)
    {
        _userHttpContext = userHttpContext;
        _logger = logger;
        _userRepository = userRepository;
        _addressRepository = addressRepository;
    }

    public async Task<Result<bool>> Handle(SetDefaultAddressCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _userHttpContext.GetUserId();

            var addressId = ShippingAddressId.Of(request.AddressId);

            var addressesResult = await _addressRepository.GetAllByFilterAsync(
                filterExpression: x => x.UserId == userId,
                cancellationToken: cancellationToken
            );

            if (addressesResult.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_addressRepository.GetAllByFilterAsync), "Failed to get user addresses", new { userId, Error = addressesResult.Error });

                return addressesResult.Error;
            }

            var addresses = addressesResult.Response!;

            var addressResult = await _addressRepository.GetByIdAsync(addressId, cancellationToken: cancellationToken);

            if (addressResult.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_addressRepository.GetByIdAsync), "Address not found", new { request.AddressId, userId, Error = addressResult.Error });

                return addressResult.Error;
            }

            var address = addressResult.Response!;

            foreach (var item in addresses)
            {
                item.IsDefault = false;
            }

            var targetAddress = addresses.FirstOrDefault(x => x.Id == address.Id);

            if (targetAddress == null)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(Handle), "Target address not found in user addresses", new { request.AddressId, userId });

                return Errors.Address.DoesNotExist;
            }

            targetAddress.IsDefault = true;

            var updateResult = await _addressRepository.UpdateRangeAsync(addresses, cancellationToken);

            if (updateResult.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_addressRepository.UpdateRangeAsync), "Failed to update addresses", new { request.AddressId, userId, Error = updateResult.Error });

                return updateResult.Error;
            }

            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully set default address", new { request.AddressId, userId });

            return updateResult.Response;
        }
        catch (Exception ex)
        {
            var userId = _userHttpContext.GetUserId();
            var parameters = new { addressId = request.AddressId, userId };
            _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);
            
            throw;
        }
    }
}
