

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.IPhone16.Commands.Extensions;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;

namespace YGZ.Catalog.Application.IPhone16.Commands.CreateIPhone16Detail;

public class CreateIPhone16DetailsCommandHandler : ICommandHandler<CreateIPhone16DetailsCommand, bool>
{
    private readonly IMongoRepository<IPhone16Detail> _repository;

    public CreateIPhone16DetailsCommandHandler(IMongoRepository<IPhone16Detail> repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(CreateIPhone16DetailsCommand request, CancellationToken cancellationToken)
    {
        IPhone16Detail newProductItem = request.ToEntity();

        await _repository.InsertOneAsync(newProductItem);

        return true;
    }
}
