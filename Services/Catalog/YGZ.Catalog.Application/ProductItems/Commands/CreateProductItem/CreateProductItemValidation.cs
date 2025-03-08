

using FluentValidation;
using JetBrains.Annotations;
using YGZ.Catalog.Domain.Core.Enums;

namespace YGZ.Catalog.Application.ProductItems.Commands.CreateProductItem;

[UsedImplicitly]
public class CreateProductItemValidation : AbstractValidator<CreateProductItemCommand>
{
    public CreateProductItemValidation()
    {
        RuleFor(product => product.Model)
           .NotEmpty()
           .MaximumLength(100);

        RuleFor(product => product.Price)
           .GreaterThan(0);

        RuleFor(product => product.Storage)
            .Must(storage => StorageEnum.List.Any(s => s.Value == storage))
            .WithMessage("Invalid storage value. Allowed values are: " +
                 string.Join(", ", StorageEnum.List.OrderBy(s => s.Value).Select(s => s.Value)));


        RuleFor(product => product.ProductId)
            .NotEmpty()
            .Must(Common.Validators.SchemaValidators.ObjectIdValidator.IsValid).WithMessage("Invalid product id");
    }
}
