

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Abstractions.HttpContext;
using YGZ.Identity.Application.Users.Extensions;

namespace YGZ.Identity.Application.Users.Commands.AddAddress;

public class AddAddressCommandHandler : ICommandHandler<AddAddressCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;

    public AddAddressCommandHandler(IUserRepository userRepository, IUserContext userContext)
    {
        _userRepository = userRepository;
        _userContext = userContext;
    }

    public async Task<Result<bool>> Handle(AddAddressCommand request, CancellationToken cancellationToken)
    {
        var userEmail = _userContext.GetUserEmail();

        var userAsync = await _userRepository.GetUserByEmailAsync(userEmail, cancellationToken);

        if (userAsync.IsFailure)
        {
            return userAsync.Error;
        }

        var newAddress = request.ToShippingAddress(userId: userAsync.Response!.Id);

        var result = await _userRepository.AddShippingAddressAsync(newAddress, userAsync.Response, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return result.Response;
    }
}
