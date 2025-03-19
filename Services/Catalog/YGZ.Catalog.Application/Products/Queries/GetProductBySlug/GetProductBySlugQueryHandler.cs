

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Catalog.Application.Products.Queries.GetProductBySlug;

public class GetProductBySlugQueryHandler : IQueryHandler<GetProductBySlugQuery, bool>
{
    public async Task<Result<bool>> Handle(GetProductBySlugQuery request, CancellationToken cancellationToken)
    {
        return true;
    }
}
