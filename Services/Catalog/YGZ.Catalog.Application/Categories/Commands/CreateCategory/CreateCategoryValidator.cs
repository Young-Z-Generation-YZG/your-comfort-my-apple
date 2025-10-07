
using FluentValidation;
using YGZ.Catalog.Application.Common.Validators;

namespace YGZ.Catalog.Application.Categories.Commands.CreateCategory;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.ParentId)
            .Must(id => id is null || SchemaValidators.ObjectIdValidator.IsValid(id)!).WithMessage("Invalid category id");
    }
}
