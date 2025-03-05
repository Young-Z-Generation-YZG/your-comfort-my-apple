

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Keycloak.Application.Abstractions.Mails;
using YGZ.Keycloak.Application.Emails.Models;

namespace YGZ.Keycloak.Application.Emails;

public class TestSendEmailCommandHandler : ICommandHandler<TestSendEmailCommand, bool>
{
    private readonly IEmailService _emailService;

    public TestSendEmailCommandHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<Result<bool>> Handle(TestSendEmailCommand request, CancellationToken cancellationToken)
    {
        var command = new EmailCommand(
                        ReceiverEmail: "lov3rinve146@gmail.com",
                        Subject: "Welcome to Our Service",
                        ViewName: "WelcomeEmail",
                        Model: new WelcomeEmailModel { UserName = "John" },
                        Attachments: null
        );

        await _emailService.SendEmailAsync(command);

        return true;
    }
}
