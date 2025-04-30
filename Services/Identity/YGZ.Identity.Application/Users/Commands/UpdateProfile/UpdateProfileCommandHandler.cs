using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Abstractions.HttpContext;
using YGZ.Identity.Domain.Core.Enums;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Application.Users.Commands.UpdateProfile;


public class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand, bool>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;

    public UpdateProfileCommandHandler(IUserContext userContext, IProfileRepository profileRepository, IUserRepository userRepository)
    {
        _userContext = userContext;
        _profileRepository = profileRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<bool>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var userEmail = _userContext.GetUserEmail();

        var userResult = await _userRepository.GetUserByEmailAsync(userEmail, cancellationToken);

        if (userResult.IsFailure)
        {
            return userResult.Error;
        }

        userResult.Response!.PhoneNumber = request.PhoneNumber;

        var updateUserResult = await _userRepository.UpdateUserAsync(userResult.Response, cancellationToken);

        if (updateUserResult.IsFailure)
        {
            return updateUserResult.Error;
        }

        var profileResult = await _profileRepository.GetProfileByUser(userResult.Response, cancellationToken);

        if (profileResult.IsFailure)
        {
            return profileResult.Error;
        }

        profileResult.Response!.Update(firstName: request.FirstName, lastName: request.LastName, birthDay: DateTime.Parse(request.BirthDay).ToUniversalTime(), gender: Gender.FromName(request.Gender));

        var updateResult = await _profileRepository.UpdateAsync(profileResult.Response, cancellationToken);

        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        return true;
    }
}
