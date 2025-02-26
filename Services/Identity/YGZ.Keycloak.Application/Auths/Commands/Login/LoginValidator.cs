

using FluentValidation;

namespace YGZ.Keycloak.Application.Auths.Commands.Login;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(register => register.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(register => register.Password)
            .NotEmpty();
    }
}
