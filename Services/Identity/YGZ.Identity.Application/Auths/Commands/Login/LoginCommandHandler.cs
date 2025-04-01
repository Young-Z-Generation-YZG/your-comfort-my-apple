
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Abstractions.Emails;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Application.Abstractions.Utils;
using YGZ.Identity.Application.Emails.Models;
using YGZ.Identity.Application.Emails;
using YGZ.Identity.Domain.Core.Enums;

namespace YGZ.Identity.Application.Auths.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly IIdentityService _identityService;
    private readonly IKeycloakService _keycloakService;
    private readonly ICachedRepository _cachedRepository;
    private readonly IOtpGenerator _otpGenerator;
    private readonly IEmailService _emailService;

    public LoginCommandHandler(ILogger<LoginCommandHandler> logger,
                               IIdentityService identityService,
                               IKeycloakService keycloakService,
                               ICachedRepository cachedRepository,
                               IOtpGenerator otpGenerator,
                               IEmailService emailService)
    {
        _logger = logger;
        _identityService = identityService;
        _keycloakService = keycloakService;
        _cachedRepository = cachedRepository;
        _otpGenerator = otpGenerator;
        _emailService = emailService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.LoginAsync(request);

        if (user.IsFailure)
        {
            return user.Error;
        }

        if (!user.Response!.EmailConfirmed)
        {
            var otp = _otpGenerator.GenerateOtp(6);

            await _cachedRepository.SetAsync(request.Email, otp, TimeSpan.FromMinutes(5));

            var tokenResult = await _identityService.GenerateEmailVerificationTokenAsync(request.Email).ConfigureAwait(false);

            if (tokenResult.IsFailure)
            {
                return tokenResult.Error;
            }

            var command = new EmailCommand(
                        ReceiverEmail: request.Email,
                        Subject: "YGZ Zone Email Verifycation",
                        ViewName: "EmailVerification",
                        Model: new EmailVerificationModel
                        {
                            FullName = $"Foo Bar",
                            VerifyOtp = otp,
                            VerificationLink = $"https://ygz.zone/verify/otp?_email={request.Email}&_token={tokenResult.Response}&_otp={otp}"
                        },
                        Attachments: null
            );

            await _emailService.SendEmailAsync(command);

            Dictionary<string, string> Params = new Dictionary<string, string>
            {
                { "_email", request.Email },
                { "_token", tokenResult.Response! }
            };

            var emailVerificationResponse = new LoginResponse
            {
                UserEmail = user.Response.Email!,
                AccessToken = null,
                RefreshToken = null,
                AccessTokenExpiresInSeconds = null,
                RefreshTokenExpiresInSeconds = null,
                VerificationType = VerificationType.EMAIL_VERIFICATION.Name,
                Params = Params,
            };

            return emailVerificationResponse;
        }

        var tokenResponse = await _keycloakService.GetKeycloackTokenPairAsync(request);

        var loginResponse = new LoginResponse
        {
            UserEmail = user.Response.Email!,
            AccessToken = tokenResponse.AccessToken,
            RefreshToken = tokenResponse.RefreshToken,
            AccessTokenExpiresInSeconds = tokenResponse.ExpiresIn,
            RefreshTokenExpiresInSeconds = tokenResponse.RefreshExpiresIn,
            VerificationType = VerificationType.CREDENTIALS_VERIFICATION.Name,
            Params = null
        };

        return loginResponse;
    }
}
