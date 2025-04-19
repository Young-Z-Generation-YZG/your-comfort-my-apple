

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Catalog.Application.Reviews.Commands;

public class DeleteReviewCommandHandler : ICommandHandler<DeleteReviewCommand, bool>
{
    public Task<Result<bool>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
