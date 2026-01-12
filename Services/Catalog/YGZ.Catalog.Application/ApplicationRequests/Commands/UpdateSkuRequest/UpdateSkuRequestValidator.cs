using FluentValidation;

namespace YGZ.Catalog.Application.Requests.Commands.UpdateSkuRequest;

public class UpdateSkuRequestValidator : AbstractValidator<UpdateSkuRequestCommand>
{
    public UpdateSkuRequestValidator()
    {
        RuleFor(x => x.SkuRequestId).NotEmpty();
        RuleFor(x => x.State).NotEmpty().Must(state => 
            state.Equals("Approved", StringComparison.OrdinalIgnoreCase) || 
            state.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
            .WithMessage("State must be 'Approved' or 'Rejected'");
    }
}
