
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Catalog.Application.IPhone16.Queries.GetIPhone16Models;

public class GetIPhone16ModelsQueryHandler : IQueryHandler<GetIPhone16ModelsQuery, bool>
{
    public async Task<Result<bool>> Handle(GetIPhone16ModelsQuery request, CancellationToken cancellationToken)
    {
        return true;
    }
}
