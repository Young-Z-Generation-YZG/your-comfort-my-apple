
using FluentValidation;

namespace YGZ.Identity.Application.Auths.Commands.Register;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(register => register.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
