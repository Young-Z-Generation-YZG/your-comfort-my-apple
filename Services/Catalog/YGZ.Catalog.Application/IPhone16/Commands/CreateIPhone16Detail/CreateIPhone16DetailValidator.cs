

using FluentValidation;
using JetBrains.Annotations;

namespace YGZ.Catalog.Application.IPhone16.Commands.CreateIPhone16Detail;

[UsedImplicitly]
public class CreateIPhone16DetailValidator : AbstractValidator<CreateIPhone16DetailsCommand>
{
    public CreateIPhone16DetailValidator()
    {
        //RuleFor(product => product.Model)
        //   .NotEmpty()
        //   .MaximumLength(100);

        //RuleFor(product => product.Price)
        //   .GreaterThan(0);

        //RuleFor(product => product.Storage)
        //    .Must(storage => Storage.List.Any(s => s.Value == storage))
        //    .WithMessage("Invalid storage value. Allowed values are: " +
        //         string.Join(", ", Storage.List.OrderBy(s => s.Value).Select(s => s.Value)));


        //RuleFor(product => product.ProductId)
        //    .NotEmpty()
        //    .Must(Common.Validators.SchemaValidators.ObjectIdValidator.IsValid).WithMessage("Invalid product id");
    }
}
