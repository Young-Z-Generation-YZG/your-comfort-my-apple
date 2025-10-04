using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Tenants;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Tenants.Commands;

public class CreateTenantHandler : ICommandHandler<CreateTenantCommand, bool>
{
    private readonly IMongoRepository<Tenant, TenantId> _repository;

    public CreateTenantHandler(IMongoRepository<Tenant, TenantId> repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenantId = TenantId.Create();

        var newBranch = Branch.Create(tenantId: tenantId,
                                      name: request.Name,
                                      description: request.BranchDescription,
                                      address: request.BranchAddress,
                                      manager: null);

        var newTenant = Tenant.Create(tenantId: tenantId,
                                      name: request.Name,
                                      description: request.TenantDescription,
                                      branch: newBranch);

        try
        {
            await _repository.StartTransactionAsync(cancellationToken);

            var result = await _repository.InsertOneAsync(newTenant);

            if (result.IsFailure)
            {
                await _repository.RollbackTransaction(cancellationToken);

                return result.Error;
            }

            await _repository.CommitTransaction(cancellationToken);

            return result.Response;
        }
        catch (Exception ex)
        {
            await _repository.RollbackTransaction(cancellationToken);

            throw;
        }
    }
}
