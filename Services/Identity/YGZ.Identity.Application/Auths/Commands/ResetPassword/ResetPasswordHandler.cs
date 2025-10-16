

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Identity.Application.Abstractions.Emails;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.Identity.Application.Emails;
using YGZ.Identity.Application.Emails.Models;
using YGZ.Identity.Domain.Core.Enums;
using YGZ.Identity.Domain.Core.Errors;

namespace YGZ.Identity.Application.Auths.Commands.ResetPassword;

public class ResetPasswordHandler : ICommandHandler<ResetPasswordCommand, ResetPasswordVerificationResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;
    private readonly string _webClientUrl;
    private readonly ILogger<RegisterHandler> _logger;
    public ResetPasswordHandler(IIdentityService identityService, IEmailService emailService, ILogger<RegisterHandler> logger, IConfiguration configuration)
    {
        _identityService = identityService;
        _emailService = emailService;
        _logger = logger;
        _webClientUrl = configuration.GetValue<string>("WebClientSettings:BaseUrl")!;
    }

    public async Task<Result<ResetPasswordVerificationResponse>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var userAsync = await _identityService.FindUserAsync(request.Email).ConfigureAwait(false);

        if (userAsync.IsFailure)
        {
            return userAsync.Error;
        }

        var tokenResult = await _identityService.GenerateResetPasswordTokenAsync(request.Email).ConfigureAwait(false);

        if (tokenResult.IsFailure)
        {
            return tokenResult.Error;
        }

        var command = new EmailCommand(
                    ReceiverEmail: request.Email,
                    Subject: "YB ZONE RESET PASSWORD",
                    ViewName: "ResetPassword",
                    Model: new ResetPasswordModel
                    {
                        FullName = $"{request.Email}",
                        ResetPasswordLink = $"{_webClientUrl}/forgot-password/reset?_email={request.Email}&_token={tokenResult.Response}"
                    },
                    Attachments: null
        );

        try
        {
            await _emailService.SendEmailAsync(command);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to send reset password email to {Email}. Error: {Error}", request.Email, ex.Message);

            return Errors.Email.FailureToSendEmail;
        }

        Dictionary<string, string> Params = new Dictionary<string, string>
        {
            { "_email", request.Email },
            { "_token", tokenResult.Response! }
        };

        var response = new ResetPasswordVerificationResponse
        {
            Params = Params,
            VerificationType = VerificationType.RESET_PASSWORD_VERIFICATION.Name,
        };

        return response;
    }
}
