using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Abstractions.Emails;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Application.Abstractions.Utils;
using YGZ.Identity.Application.BuilderClasses;
using YGZ.Identity.Application.Emails;
using YGZ.Identity.Application.Emails.Models;
using YGZ.Identity.Domain.Core.Enums;
using YGZ.Identity.Domain.Core.Errors;

namespace YGZ.Identity.Application.Auths.Commands.Register;

public class RegisterHandler : ICommandHandler<RegisterCommand, EmailVerificationResponse>
{
    private readonly ILogger<RegisterHandler> _logger;
    private readonly IIdentityService _identityService;
    private readonly IKeycloakService _keycloakService;
    private readonly ICachedRepository _cachedRepository;
    private readonly IOtpGenerator _otpGenerator;
    private readonly IEmailService _emailService;
    private readonly string _webClientUrl;
    private const string DEFAULT_TENANT_ID = "664355f845e56534956be32b";
    private const string DEFAULT_BRANCH_ID = "664357a235e84033bbd0e6b6";

    public RegisterHandler(ILogger<RegisterHandler> logger,
                           IIdentityService identityService,
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

        var userResult = await _identityService.FindUserByEmailAsync(request.Email);

        if(userResult.Response is not null)
        {
            return Errors.User.AlreadyExists;
        }

        var userRepresentation = UserRepresentation.CreateBuilder()
            .WithUsername(request.Email)
            .WithEmail(request.Email)
            .WithEnabled(true)
            .WithFirstName(request.FirstName)
            .WithLastName(request.LastName)
            .WithEmailVerified(false)
            .WithPassword(request.Password)
            .WithTenantAttributes(
                tenantId: DEFAULT_TENANT_ID,
                subDomain: null,
                tenantType: null,
                branchId: DEFAULT_BRANCH_ID,
                fullName: $"{request.FirstName} {request.LastName}"
            )
            .Build();

        var keycloakUserId = await _keycloakService.CreateKeycloakUserAsync(userRepresentation);

        var createResult = await _identityService.CreateUserAsync(request, new Guid(keycloakUserId));

        if (createResult.IsFailure)
        {
            // rollback keycloak user
            await _keycloakService.DeleteKeycloakUserAsync(keycloakUserId);

            return Errors.User.FailedToRegister;
        }

        var otp = _otpGenerator.GenerateOtp(6);

        await _cachedRepository.SetAsync(request.Email, otp, TimeSpan.FromMinutes(5));

        var emailTokenResult = await _identityService.GenerateEmailVerificationTokenAsync(keycloakUserId!).ConfigureAwait(false);

        if (emailTokenResult.IsFailure)
        {
            // rollback keycloak user
            await _keycloakService.DeleteKeycloakUserAsync(keycloakUserId);
            // rollback user
            await _identityService.DeleteUserAsync(keycloakUserId);

            return Errors.User.FailedToRegister;
        }

        var command = new EmailCommand(
                    ReceiverEmail: request.Email,
                    Subject: "YGZ Zone Email Verification",
                    ViewName: "EmailVerification",
                    Model: new EmailVerificationModel
                    {
                        FullName = $"{request.FirstName} {request.LastName}",
                        VerifyOtp = otp,
                        VerificationLink = $"{_webClientUrl}/verify/otp?_email={request.Email}&_token={emailTokenResult.Response}&_otp={otp}"
                    },
                    Attachments: null
        );

        try
        {
            await _emailService.SendEmailAsync(command);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to send email: {ErrorMessage}", ex.Message);
            
            // rollback keycloak user
            await _keycloakService.DeleteKeycloakUserAsync(keycloakUserId);
            // rollback user
            await _identityService.DeleteUserAsync(keycloakUserId);

            return Errors.User.FailedToRegister;
        }

        Dictionary<string, string> Params = new Dictionary<string, string>
        {
            { "_email", request.Email },
            { "_token", emailTokenResult.Response! }
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
