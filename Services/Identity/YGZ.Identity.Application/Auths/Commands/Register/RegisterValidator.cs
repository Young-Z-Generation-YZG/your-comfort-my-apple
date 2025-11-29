

using FluentValidation;
using YGZ.BuildingBlocks.Shared.Constants;

namespace YGZ.Identity.Application.Auths.Commands.Register;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(register => register.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(register => register.LastName)
            .NotEmpty();

        RuleFor(register => register.FirstName)
            .NotEmpty();

        RuleFor(register => register.Password)
            .MinimumLength(6);

        RuleFor(register => register.ConfirmPassword)
            .Equal(register => register.Password);

        RuleFor(register => register.PhoneNumber)
            .Matches("^\\d+$")
            .WithMessage("Phone number must contain digits only.");

        RuleFor(register => register.BirthDate)
            .NotEmpty()
            .Must(date => DateTime.TryParse(date, out _))
            .WithMessage("Birth date must be a valid date.");

            RuleFor(register => register.Country)
                .NotEmpty()
                .Must(country => CountryConstants.Countries.Contains(country))
                .WithMessage("Country not available.");
    }
}
