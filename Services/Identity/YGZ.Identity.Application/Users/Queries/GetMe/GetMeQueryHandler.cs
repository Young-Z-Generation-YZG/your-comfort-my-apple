

using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Abstractions.HttpContext;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Users.Entities;

namespace YGZ.Identity.Application.Users.Queries.GetProfile;

public class GetMeQueryHandler : IQueryHandler<GetMeQuery, GetAllInfoResposne>
{
    private readonly IUserRequestContext _userContext;
    private readonly ILogger<GetMeQueryHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IProfileRepository _profileRepository;

    public GetMeQueryHandler(IUserRequestContext userContext, ILogger<GetMeQueryHandler> logger, IUserRepository userRepository, IProfileRepository profileRepository)
    {
        _userContext = userContext;
        _logger = logger;
        _userRepository = userRepository;
        _profileRepository = profileRepository;
    }

    public async Task<Result<GetAllInfoResposne>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var userEmail = _userContext.GetUserEmail();

        var expressions = new Expression<Func<User, object>>[]
        {
            x => x.Profile,
            x => x.ShippingAddresses.Where(sa => sa.IsDefault == true),
        };

        var userAsync = await _userRepository.GetUserByEmailAsync(userEmail, cancellationToken, expressions);

        if (userAsync.IsFailure)
        {
            return userAsync.Error;
        }

        GetAllInfoResposne response = MapToResponse(userAsync.Response!);

        return response;
    }

    private GetAllInfoResposne MapToResponse(User user)
    {
        var defaultAddress = user.ShippingAddresses.FirstOrDefault(x => x.IsDefault)!;

        return new GetAllInfoResposne
        {
            Email = user.Email!,
            FirstName = user.Profile.FirstName,
            LastName = user.Profile.LastName,
            PhoneNumber = user.PhoneNumber!,
            BirthDate = user.Profile.BirthDay.ToString("yyyy-MM-dd"),
            ImageId = user.Profile.Image!.ImageId,
            ImageUrl = user.Profile.Image.ImageUrl,
            DefaultAddressLabel = defaultAddress?.Label ?? "",
            DefaultContactName = defaultAddress?.ContactName ?? "",
            DefaultContactPhoneNumber = defaultAddress?.ContactPhoneNumber ?? "",
            DefaultAddressLine = defaultAddress?.AddressDetail.AddressLine ?? "",
            DefaultAddressDistrict = defaultAddress?.AddressDetail.AddressDistrict ?? "",
            DefaultAddressProvince = defaultAddress?.AddressDetail.AddressProvince ?? "",
            DefaultAddressCountry = defaultAddress?.AddressDetail.AddressCountry ?? "",
        };
    }
}
