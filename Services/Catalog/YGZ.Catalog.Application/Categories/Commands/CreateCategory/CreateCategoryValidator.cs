
using FluentValidation;

namespace YGZ.Catalog.Application.Categories.Commands.CreateCategory;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.ParentId).Null();
    }
}
