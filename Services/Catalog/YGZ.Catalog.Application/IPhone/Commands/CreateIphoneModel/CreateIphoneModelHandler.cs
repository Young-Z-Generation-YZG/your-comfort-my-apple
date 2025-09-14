
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Application.IPhone.Extensions;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone;

namespace YGZ.Catalog.Application.IPhone.Commands.CreateIphoneModel;

public class CreateIPhoneModelCommandHandler : ICommandHandler<CreateIphoneModelCommand, bool>
{
    private readonly IMongoRepository<IphoneModel, ModelId> _repository;

    public CreateIPhoneModelCommandHandler(IMongoRepository<IphoneModel, ModelId> repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(CreateIphoneModelCommand request, CancellationToken cancellationToken)
    {
        IphoneModel newModel = request.ToEntity();

        var result = await _repository.InsertOneAsync(newModel);

        return result.Response;
    }
}
