
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Catalog.Application.Reviews.Commands;

public class UpdateReviewCommandHandler : ICommandHandler<UpdateReviewCommand, bool>
{
    public Task<Result<bool>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
