using FluentValidation;

namespace YGZ.Catalog.Application.Requests.Commands.CreateSkuRequest;

public class CreateSkuRequestValidator : AbstractValidator<CreateSkuRequestCommand>
{
    public CreateSkuRequestValidator()
    {
        RuleFor(x => x.SenderUserId).NotEmpty();
        RuleFor(x => x.FromBranchId).NotEmpty();
        RuleFor(x => x.ToBranchId).NotEmpty();
        RuleFor(x => x.SkuId).NotEmpty();
        RuleFor(x => x.RequestQuantity).GreaterThan(0);
    }
}
