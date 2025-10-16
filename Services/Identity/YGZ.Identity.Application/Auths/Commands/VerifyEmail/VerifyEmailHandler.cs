
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Domain.Core.Errors;

namespace YGZ.Identity.Application.Auths.Commands.VerifyEmail;

public class VerifyEmailHandler : ICommandHandler<VerifyEmailCommand, bool>
{
    private readonly IIdentityService _identityService;
    private readonly IKeycloakService _keycloakService;
    private readonly ICachedRepository _cachedRepository;

    public VerifyEmailHandler(IIdentityService identityService, IKeycloakService keycloakService, ICachedRepository cachedRepository)
    {
        _identityService = identityService;
        _keycloakService = keycloakService;
        _cachedRepository = cachedRepository;
    }

    public async Task<Result<bool>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var otp = await _cachedRepository.GetAsync(request.Email);

        if (otp is null)
        {
            return Errors.Auth.ExpiredToken;
        }

        if (otp != request.Otp)
        {
            return Errors.Auth.InvalidToken;
        }

        var result = await _identityService.VerifyEmailTokenAsync(request.Email, request.Token);

        if(result.IsFailure)
        {
            return result.Error;
        }

        var verifyKeycloakUser = await _keycloakService.VerifyEmailAsync(request.Email);

        if (verifyKeycloakUser.IsFailure)
        {
            return verifyKeycloakUser.Error;
        }

        await _cachedRepository.RemoveAsync(request.Email);

        return result.Response;
    }
}
