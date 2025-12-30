
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Users.Queries.GetProfile;
using YGZ.Identity.Domain.Users.Entities;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Application.Users.Commands.DeleteAddress;

public class DeleteAddressHandler : ICommandHandler<DeleteAddressCommand, bool>
{
    private readonly ILogger<DeleteAddressHandler> _logger;
    private readonly IGenericRepository<ShippingAddress, ShippingAddressId> _addressRepository;

    public DeleteAddressHandler(ILogger<DeleteAddressHandler> logger,
                                IGenericRepository<ShippingAddress, ShippingAddressId> addressRepository)
    {
        _logger = logger;
        _addressRepository = addressRepository;
    }

    public async Task<Result<bool>> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var addressId = ShippingAddressId.Of(request.AddressId);

            var result = await _addressRepository.GetByIdAsync(addressId, cancellationToken: cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_addressRepository.GetByIdAsync), "Address not found", new { request.AddressId, Error = result.Error });

                return result.Error;
            }

            var removeResult = await _addressRepository.RemoveAsync(result.Response!, cancellationToken);

            if (removeResult.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_addressRepository.RemoveAsync), "Failed to delete address", new { request.AddressId, Error = removeResult.Error });

                return removeResult.Error;
            }

            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully deleted address", new { request.AddressId });

            return removeResult.Response;
        }
        catch (Exception ex)
        {
            var parameters = new { addressId = request.AddressId };
            _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);
            
            throw;
        }
    }
}
