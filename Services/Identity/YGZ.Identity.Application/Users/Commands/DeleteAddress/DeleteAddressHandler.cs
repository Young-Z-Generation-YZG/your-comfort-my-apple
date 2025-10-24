
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
    private readonly ILogger<GetMeQueryHandler> _logger;
    private readonly IGenericRepository<ShippingAddress, ShippingAddressId> _addressRepository;

    public DeleteAddressHandler(ILogger<GetMeQueryHandler> logger,
                                IGenericRepository<ShippingAddress, ShippingAddressId> addressRepository)
    {
        _logger = logger;
        _addressRepository = addressRepository;
    }

    public async Task<Result<bool>> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var addressId = ShippingAddressId.Of(request.AddressId);

        var result = await _addressRepository.GetByIdAsync(addressId, cancellationToken: cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var removeResult = await _addressRepository.RemoveAsync(result.Response!, cancellationToken);

        if (removeResult.IsFailure)
        {
            return removeResult.Error;
        }

        return removeResult.Response;
    }
}
