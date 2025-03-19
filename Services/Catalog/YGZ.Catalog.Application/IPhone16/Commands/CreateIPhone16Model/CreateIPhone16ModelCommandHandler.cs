

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.IPhone16.Commands.Extensions;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Iphone16;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;

namespace YGZ.Catalog.Application.IPhone16.Commands.CreateIPhone16Model;

public class CreateIPhone16ModelCommandHandler : ICommandHandler<CreateIPhone16ModelCommand, bool>
{
    private readonly IMongoRepository<IPhone16Model> _repository;

    public CreateIPhone16ModelCommandHandler(IMongoRepository<IPhone16Model> repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(CreateIPhone16ModelCommand request, CancellationToken cancellationToken)
    {
        IPhone16Model newProductItem = request.ToEntity();

        await _repository.InsertOneAsync(newProductItem);

        return true;
    }
}
