using Microsoft.Extensions.Options;
using YGZ.Identity.Application.Core.Abstractions.Emails;
using YGZ.Identity.Application.Core.Abstractions.Identity;
using YGZ.Identity.Application.Core.Abstractions.Messaging;
using YGZ.Identity.Application.Emails.Commands;
using YGZ.Identity.Domain.Common.Abstractions;
using YGZ.Identity.Domain.Core.Configs;

namespace YGZ.Identity.Application.Identity.Commands.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, bool>
{
    private readonly IIdentityService _identityService;
    private readonly IEmailNotificationService _emailNotificationService;
    private readonly AppConfig _options;

    public CreateUserCommandHandler(
            IIdentityService identityService,
            IEmailNotificationService emailNotificationService,
            IOptions<AppConfig> options)
    {
        _identityService = identityService;
        _emailNotificationService = emailNotificationService;
        _options = options.Value;
    }

    public async Task<Result<bool>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.CreateUserAsync(request);

        if(result.IsFailure)
        {
            return result.Error;
        }

        Console.WriteLine("User created successfully", result);

        var searchResult = await _identityService.FindUserAsync(new(request.Email));

        if (searchResult.IsFailure)
        {
            return searchResult.Error;
        }

        var verificationTokenResult = await _identityService.GenerateEmailVerificationTokenAsync(request.Email);

        Console.WriteLine("Verification token generated successfully", verificationTokenResult);

        if (verificationTokenResult.IsFailure)
        {
            return verificationTokenResult.Error;
        }

        await _emailNotificationService.SendEmailConfirmation(
            new EmailConfirmationCommand(
                request.Email, 
                searchResult.Response!.NormalizedUserName!, 
                $"{_options.FrontendConfig.Url}/verify?email={request.Email}&token={verificationTokenResult.Response}")
            );


        return true;
    }
}
