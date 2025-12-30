using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Constants;
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
    private readonly IUserRegistrationCompensator _compensator;
    private readonly string _webClientUrl;
    private const string DEFAULT_TENANT_ID = "664355f845e56534956be32b";
    private const string DEFAULT_BRANCH_ID = "664357a235e84033bbd0e6b6";

    public RegisterHandler(ILogger<RegisterHandler> logger,
                           IIdentityService identityService,
                           IKeycloakService keycloakService,
                           IOtpGenerator otpGenerator,
                           IEmailService emailService,
                           ICachedRepository cachedRepository,
                           IUserRegistrationCompensator compensator,
                           IConfiguration configuration)
    {
        _logger = logger;
        _identityService = identityService;
        _keycloakService = keycloakService;
        _cachedRepository = cachedRepository;
        _emailService = emailService;
        _otpGenerator = otpGenerator;
        _compensator = compensator;
        _webClientUrl = configuration.GetValue<string>("WebClientSettings:BaseUrl")!;
    }

    public async Task<Result<EmailVerificationResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Step 1: Check if user already exists
            var userResult = await _identityService.FindUserByEmailAsync(request.Email);

            if (userResult.IsSuccess && userResult.Response is not null)
            {
                _logger.LogWarning(":::[Handler Warning]::: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                    nameof(_identityService.FindUserByEmailAsync), "User already exists", new { request.Email });
                
                return Errors.User.AlreadyExists;
            }

            // Step 2: Create user in Keycloak
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

            string keycloakUserId;
            try
            {
                keycloakUserId = await _keycloakService.CreateKeycloakUserAsync(userRepresentation);

                _compensator.TrackStep(RegistrationStep.KeycloakUserCreated, keycloakUserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_keycloakService.CreateKeycloakUserAsync), "Failed to create user in Keycloak", new { request.Email });
                
                return Errors.User.FailedToRegister;
            }

            // Step 3: Create user in database
            var createResult = await _identityService.CreateUserAsync(request, new Guid(keycloakUserId));

            if (createResult.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_identityService.CreateUserAsync), "Failed to create user in database", createResult.Error);

                await _compensator.CompensateAsync(keycloakUserId, null, cancellationToken);

                return Errors.User.FailedToRegister;
            }

            var createdUser = createResult.Response!;
            _compensator.TrackStep(RegistrationStep.DatabaseUserCreated, keycloakUserId);

            // Step 4: Assign default role to user
            var roleAssignmentResult = await _identityService.AssignRolesAsync(
                createdUser,
                new List<string> { AuthorizationConstants.Roles.USER });

            if (roleAssignmentResult.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_identityService.AssignRolesAsync), "Failed to assign default role to user", roleAssignmentResult.Error);

                await _compensator.CompensateAsync(keycloakUserId, keycloakUserId, cancellationToken);

                return Errors.User.FailedToRegister;
            }

            _compensator.TrackStep(RegistrationStep.RoleAssigned, createdUser.Id);

            // Step 5: Generate and cache OTP
            var otp = _otpGenerator.GenerateOtp(6);
            await _cachedRepository.SetAsync(request.Email, otp, TimeSpan.FromMinutes(5));
            _compensator.TrackStep(RegistrationStep.OtpCached, request.Email);

            // Step 6: Generate email verification token
            var emailTokenResult = await _identityService.GenerateEmailVerificationTokenAsync(createdUser);

            if (emailTokenResult.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_identityService.GenerateEmailVerificationTokenAsync), "Failed to generate email verification token", emailTokenResult.Error);

                await _compensator.CompensateAsync(keycloakUserId, keycloakUserId, cancellationToken);

                return Errors.User.FailedToRegister;
            }

            _compensator.TrackStep(RegistrationStep.EmailTokenGenerated, emailTokenResult.Response!);

            // Step 7: Send verification email
            var emailCommand = new EmailCommand(
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
                await _emailService.SendEmailAsync(emailCommand);

                _compensator.TrackStep(RegistrationStep.EmailSent, request.Email);
            }
            catch (Exception)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_emailService.SendEmailAsync), "Failed to send email", new { request.Email });

                // Email sending failure - still compensate, but note that email is idempotent
                await _compensator.CompensateAsync(keycloakUserId, keycloakUserId, cancellationToken);
                return Errors.User.FailedToRegister;
            }

            // Success - build response
            var response = new EmailVerificationResponse
            {
                Params = new Dictionary<string, string>
                {
                    { "_email", request.Email },
                    { "_token", emailTokenResult.Response! }
                },
                VerificationType = VerificationType.EMAIL_VERIFICATION.Name,
                TokenExpiredIn = TimeSpan.FromMinutes(5).Seconds
            };

            return response;
        }
        catch (Exception ex)
        {
            var parameters = new { email = request.Email };
            _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);

            // Attempt compensation based on current state
            var state = _compensator.GetState();
            await _compensator.CompensateAsync(state.KeycloakUserId, state.DatabaseUserId, cancellationToken);

            return Errors.User.FailedToRegister;
        }
    }
}
