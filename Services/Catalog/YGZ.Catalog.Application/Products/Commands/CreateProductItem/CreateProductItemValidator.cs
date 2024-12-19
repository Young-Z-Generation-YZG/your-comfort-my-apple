
using FluentValidation;
using MongoDB.Bson;
using YGZ.Catalog.Application.Common.CustomValidator;
using YGZ.Catalog.Domain.Core.Enums;

namespace YGZ.Catalog.Application.Products.Commands.CreateProductItem;

public class CreateProductItemValidator : AbstractValidator<CreateProductItemCommand>
{
    public CreateProductItemValidator()
    {
        RuleFor(product => product.Model)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(product => product.Color)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(product => product.Storage)
            .Must(storage => StorageEnum.List.Any(s => s.Value == storage))
            .WithMessage("Invalid storage value. Allowed values are: " +
                 string.Join(", ", StorageEnum.List.OrderBy(s => s.Value).Select(s => s.Value)));

        RuleFor(product => product.Price)
            .GreaterThan(0);

        RuleFor(product => product.Quantity_in_stock)
            .GreaterThan(0);

        RuleFor(product => product.ProductId)
            .NotEmpty()
            .Must(Validators.ObjectIdVlidator.BeAValidObjectId).WithMessage("Invalid product id");

        RuleFor(product => product.PromotionId)
            .Must(Validators.ObjectIdVlidator.BeAValidObjectId)
            .WithMessage("Invalid promotion id")
            .When(product => product.PromotionId != null);
    }
}
