
using FluentValidation;
using YGZ.Identity.Domain.Common.Constants.Errors;
using YGZ.Identity.Domain.Identity.Constants.Regex;

namespace YGZ.Identity.Application.Identity.Commands.Login;
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(user => user.Email)
            .Cascade(CascadeMode.Continue)
            .NotEmpty()
            .WithErrorCode(ErrorCode.PropertyError("Email"))
            .WithMessage(ErrorMessage.PropertyEmpty("Email"))
            .NotNull()
            .WithErrorCode(ErrorCode.PropertyError("Email"))
            .WithMessage(ErrorMessage.PropertyEmpty("Email"))
            .Matches(Patterns.Email)
            .WithErrorCode(ErrorCode.PropertyError("Email"))
            .WithMessage(ErrorMessage.PropertyError("Email"));
    }
}
