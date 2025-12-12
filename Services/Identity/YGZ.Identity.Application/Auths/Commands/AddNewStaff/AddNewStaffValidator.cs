using FluentValidation;

namespace YGZ.Identity.Application.Auths.Commands.AddNewStaff;

public class AddNewStaffValidator : AbstractValidator<AddNewStaffCommand>
{
    public AddNewStaffValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be a valid email address");

        //RuleFor(x => x.Password)
        //    .NotEmpty()
        //    .WithMessage("Password is required")
        //    .MinimumLength(8)
        //    .WithMessage("Password must be at least 8 characters long")
        //    .Matches(@"[A-Z]")
        //    .WithMessage("Password must contain at least one uppercase letter")
        //    .Matches(@"[a-z]")
        //    .WithMessage("Password must contain at least one lowercase letter")
        //    .Matches(@"[0-9]")
        //    .WithMessage("Password must contain at least one digit");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(100)
            .WithMessage("First name must not exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(100)
            .WithMessage("Last name must not exceed 100 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .Matches(@"^\+?[\d\s\-()]+$")
            .WithMessage("Phone number must be in a valid format (e.g., +1-234-567-8900 or 0123456789)");

        RuleFor(x => x.RoleName)
            .NotEmpty()
            .WithMessage("Role name is required");

        RuleFor(x => x.BirthDay)
            .NotEmpty()
            .WithMessage("Birth day is required");
    }
}

