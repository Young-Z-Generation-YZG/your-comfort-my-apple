
using FluentValidation;
using YGZ.Identity.Domain.Common.Constants.Errors;
using YGZ.Identity.Domain.Identity.Constants.Regex;

namespace YGZ.Identity.Application.Identity.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(user => user.FirstName)
            //.Cascade(CascadeMode.Continue)
            //.NotEmpty()
            //.WithErrorCode(ErrorCode.PropertyError("FirstName"))
            //.WithMessage(ErrorMessage.PropertyEmpty("First Name"))
            .NotNull()
            .WithErrorCode(ErrorCode.PropertyError("FirstName"))
            .WithMessage(ErrorMessage.PropertyEmpty("First Name"))
            .Length(2, 50)
            .WithErrorCode(ErrorCode.PropertyError("FirstName"))
            .WithMessage(ErrorMessage.PropertyLength("First Name", 2, 50));
        //.Matches(Pattern.OnlyAlphabeticCharactersWithApostrophie)
        //.WithErrorCode(ErrorCode.PropertyError("FirstName"))
        //.WithMessage(ErrorMessage.InvalidCharacters("First Name"));

        RuleFor(user => user.LastName)
            //.Cascade(CascadeMode.Continue)
            //.NotEmpty()
            //.WithErrorCode(ErrorCode.PropertyError("LastName"))
            //.WithMessage(ErrorMessage.PropertyEmpty("Last Name"))
            .NotNull()
            .WithErrorCode(ErrorCode.PropertyError("LastName"))
            .WithMessage(ErrorMessage.PropertyEmpty("Last Name"))
            .Length(2, 50)
            .WithErrorCode(ErrorCode.PropertyError("LastName"))
            .WithMessage(ErrorMessage.PropertyLength("Last Name", 2, 50));
        //.Matches(Pattern.OnlyAlphabeticCharactersWithApostrophie)
        //.WithErrorCode(ErrorCode.PropertyError("LastName"))
        //.WithMessage(ErrorMessage.InvalidCharacters("Last Name"));

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

        RuleFor(user => user.Password)
            .NotNull()
            .WithErrorCode(ErrorCode.PropertyError("Password"))
            .WithMessage(ErrorMessage.PropertyEmpty("Password"))
            .Length(5, 15)
            .WithErrorCode(ErrorCode.PropertyError("Password"))
            .WithMessage(ErrorMessage.PropertyLength("Password", 5, 15));
    }
}
