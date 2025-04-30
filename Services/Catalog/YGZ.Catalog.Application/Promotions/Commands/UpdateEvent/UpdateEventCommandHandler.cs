
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Promotions.Commands.UpdatePromotionEvent;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Catalog.Application.Promotions.Commands.UpdateEvent;

public class UpdateEventCommandHandler : ICommandHandler<UpdateEventCommand, bool>
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public UpdateEventCommandHandler(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<Result<bool>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
