
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Emails;
using YGZ.Identity.Application.Emails.Models;

namespace YGZ.Identity.Application.Emails;

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
                        ViewName: "EmailVerification",
                        Model: new EmailVerificationModel
                        {
                            FullName = "Foo Bar",
                            VerifyOtp = "332452",
                            VerificationLink = "https://ygz.zone/verify/otp?_q=\"jwt\"&_verifyType=email"
                        },
                        Attachments: null
        );

        await _emailService.SendEmailAsync(command);

        return true;
    }
}
