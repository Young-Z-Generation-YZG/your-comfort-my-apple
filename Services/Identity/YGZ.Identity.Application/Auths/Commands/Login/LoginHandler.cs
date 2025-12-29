
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Abstractions.Emails;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Application.Abstractions.Utils;
using YGZ.Identity.Application.Emails;
using YGZ.Identity.Application.Emails.Models;
using YGZ.Identity.Domain.Core.Enums;

namespace YGZ.Identity.Application.Auths.Commands.Login;

public class LoginHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly ILogger<LoginHandler> _logger;
    private readonly IIdentityService _identityService;
    private readonly IKeycloakService _keycloakService;
    private readonly ICachedRepository _cachedRepository;
    private readonly IOtpGenerator _otpGenerator;
    private readonly IEmailService _emailService;
    private const int OTP_TTL = 5;

    public LoginHandler(ILogger<LoginHandler> logger,
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
        var userResult = await _identityService.FindUserAsync(request.Email);

        if (userResult.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_identityService.FindUserAsync), "User does not exist", userResult.Error);

            return userResult.Error; // Errors.User.DoesNotExist
        }

        var user = userResult.Response!;

        var loginResult = _identityService.Login(user, request.Password);

        if (loginResult.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_identityService.Login), "Invalid credentials", loginResult.Error);

            return loginResult.Error; // Errors.User.InvalidCredentials
        }

        if (!user.EmailConfirmed)
        {
            var otp = _otpGenerator.GenerateOtp(6);

            await _cachedRepository.SetAsync(request.Email, otp, TimeSpan.FromMinutes(OTP_TTL));

            var emailVerificationTokenResult = await _identityService.GenerateEmailVerificationTokenAsync(user);

            if (emailVerificationTokenResult.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_identityService.GenerateEmailVerificationTokenAsync), "Invalid token", emailVerificationTokenResult.Error);

                return emailVerificationTokenResult.Error; // Errors.Auth.InvalidToken
            }

            string emailVerificationToken = emailVerificationTokenResult.Response!;

            var command = new EmailCommand(
                        ReceiverEmail: request.Email,
                        Subject: "YGZ Zone Email Verification",
                        ViewName: "EmailVerification",
                        Model: new EmailVerificationModel
                        {
                            FullName = $"{user.Email}",
                            VerifyOtp = otp,
                            VerificationLink = $"https://ygz.zone/verify/otp?_email={request.Email}&_token={emailVerificationToken}&_otp={otp}"
                        },
                        Attachments: null
            );

            try
            {
                await _emailService.SendEmailAsync(command).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_emailService.SendEmailAsync), "Failed to send email", ex.Message);

                throw;
            }

            Dictionary<string, string> Params = new Dictionary<string, string>
            {
                { "_email", request.Email },
                { "_token", emailVerificationToken }
            };

            var emailVerificationResponse = new LoginResponse
            {
                UserEmail = user.Email!,
                Username = $"{user.UserName}",
                AccessToken = null,
                RefreshToken = null,
                AccessTokenExpiresInSeconds = null,
                RefreshTokenExpiresInSeconds = null,
                VerificationType = VerificationType.EMAIL_VERIFICATION.Name,
                Params = Params,
            };

            return emailVerificationResponse;
        }

        var tokenPair = await _keycloakService.GetKeycloakTokenPairAsync(request.Email, request.Password);

        var loginResponse = new LoginResponse
        {
            UserEmail = user.Email!,
            Username = user.UserName!,
            AccessToken = tokenPair.AccessToken,
            RefreshToken = tokenPair.RefreshToken,
            AccessTokenExpiresInSeconds = tokenPair.ExpiresIn,
            RefreshTokenExpiresInSeconds = tokenPair.RefreshExpiresIn,
            VerificationType = VerificationType.CREDENTIALS_VERIFICATION.Name,
            Params = null
        };

        return loginResponse;
    }
}
