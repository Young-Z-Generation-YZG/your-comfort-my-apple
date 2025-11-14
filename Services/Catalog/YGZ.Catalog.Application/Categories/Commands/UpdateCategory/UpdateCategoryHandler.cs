using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;

namespace YGZ.Catalog.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryHandler : ICommandHandler<UpdateCategoryCommand, bool>
{
    private readonly ILogger<UpdateCategoryHandler> _logger;
    private readonly IMongoRepository<Category, CategoryId> _mongoRepository;

    public UpdateCategoryHandler(ILogger<UpdateCategoryHandler> logger, IMongoRepository<Category, CategoryId> mongoRepository)
    {
        _logger = logger;
        _mongoRepository = mongoRepository;
    }

    public async Task<Result<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
