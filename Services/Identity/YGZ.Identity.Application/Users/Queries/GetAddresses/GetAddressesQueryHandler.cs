
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Abstractions.HttpContext;
using YGZ.Identity.Domain.Users.Entities;

namespace YGZ.Identity.Application.Users.Queries.GetAddresses;

public class GetAddressesQueryHandler : IQueryHandler<GetAddressesQuery, List<AddressResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IUserRequestContext _userContext;

    public GetAddressesQueryHandler(IUserRepository userRepository, IUserRequestContext userContext, IAddressRepository shippingAddressRepository)
    {
        _userRepository = userRepository;
        _userContext = userContext;
        _addressRepository = shippingAddressRepository;
    }

    public async Task<Result<List<AddressResponse>>> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
    {
        var userEmail = _userContext.GetUserEmail();

        var userAsync = await _userRepository.GetUserByEmailAsync(userEmail, cancellationToken);

        if (userAsync.IsFailure)
        {
            return userAsync.Error;
        }

        var result = await _addressRepository.GetAllByUser(userAsync.Response!);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var response = MapToResponse(result.Response!);

        return response;
    }

    private List<AddressResponse> MapToResponse(List<ShippingAddress> shippingAddresses)
    {
        return shippingAddresses.Select(x => new AddressResponse
        {
            Id = x.Id.Value.ToString(),
            Label = x.Label,
            ContactName = x.ContactName,
            ContactPhoneNumber = x.ContactPhoneNumber,
            AddressLine = x.AddressDetail.AddressLine,
            District = x.AddressDetail.AddressDistrict,
            Province = x.AddressDetail.AddressProvince,
            Country = x.AddressDetail.AddressCountry,
            IsDefault = x.IsDefault
        }).ToList();
    }
}
