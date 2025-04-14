using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Emails;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Application.Abstractions.Utils;
using YGZ.Identity.Application.Emails.Models;
using YGZ.Identity.Application.Emails;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;
using YGZ.Identity.Domain.Core.Enums;
using YGZ.Identity.Application.Abstractions.Data;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace YGZ.Identity.Application.Auths.Commands.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, EmailVerificationResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IKeycloakService _keycloakService;
    private readonly ICachedRepository _cachedRepository;
    private readonly IOtpGenerator _otpGenerator;
    private readonly IEmailService _emailService;
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly string _webClientUrl;

    public RegisterCommandHandler(IIdentityService identityService,
                                  ILogger<RegisterCommandHandler> logger,
                                  IKeycloakService keycloakService,
                                  IOtpGenerator otpGenerator,
                                  IEmailService emailService,
                                  ICachedRepository cachedRepository,
                                  IConfiguration configuration)
    {
        _logger = logger;
        _identityService = identityService;
        _keycloakService = keycloakService;
        _cachedRepository = cachedRepository;
        _emailService = emailService;
        _otpGenerator = otpGenerator;
        _webClientUrl = configuration.GetValue<string>("WebClientSettings:BaseUrl")!;
    }

    public async Task<Result<EmailVerificationResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var keycloakUser = await _keycloakService.GetUserByUsernameOrEmailAsync(request.Email);

        if (keycloakUser.Response is not null)
        {
            return Errors.Keycloak.UserAlreadyExist;
        }

        var keycloakResult = await _keycloakService.CreateKeycloakUserAsync(request);

        if (keycloakResult.IsFailure)
        {
            return keycloakResult.Error;
        }

        var userResult = await _identityService.CreateUserAsync(request, new Guid(keycloakResult.Response!));

        if (userResult.IsFailure)
        {
            return userResult.Error;
        }

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
                        FullName = $"{request.FirstName} {request.LastName}",
                        VerifyOtp = otp,
                        VerificationLink = $"{_webClientUrl}/verify/otp?_email={request.Email}&_token={tokenResult.Response}&_otp={otp}"
                    },
                    Attachments: null
        );

        await _emailService.SendEmailAsync(command);

        Dictionary<string, string> Params = new Dictionary<string, string>
        {
            { "_email", request.Email },
            { "_token", tokenResult.Response! }
        };

        var response = new EmailVerificationResponse
        {
            Params = Params,
            VerificationType = VerificationType.EMAIL_VERIFICATION.Name,
            TokenExpiredIn = TimeSpan.FromMinutes(5).Seconds
        };

        return response;
    }
}
