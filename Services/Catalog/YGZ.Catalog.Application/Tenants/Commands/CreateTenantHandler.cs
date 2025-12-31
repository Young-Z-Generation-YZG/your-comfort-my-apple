using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Tenants;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Tenants.Commands;

public class CreateTenantHandler : ICommandHandler<CreateTenantCommand, bool>
{
    private readonly ILogger<CreateTenantHandler> _logger;
    private readonly IMongoRepository<Tenant, TenantId> _repository;

    public CreateTenantHandler(IMongoRepository<Tenant, TenantId> repository, ILogger<CreateTenantHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenantId = TenantId.Create();

        var newBranch = Branch.Create(branchId: BranchId.Create(),
                                      tenantId: tenantId,
                                      name: request.Name,
                                      description: request.BranchDescription,
                                      address: request.BranchAddress,
                                      manager: null);

        ETenantType.TryFromName(request.TenantType, out var tenantTypeEnum);

        if (tenantTypeEnum is null)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Invalid tenant type", new { tenantType = request.TenantType, name = request.Name, subDomain = request.SubDomain });

            throw new ArgumentException(
                $"Invalid tenant type: {request.TenantType}",
                nameof(request.TenantType)
            );
        }

        var newTenant = Tenant.Create(tenantId: tenantId,
                                      name: request.Name,
                                      subDomain: request.SubDomain, 
                                      tenantType: tenantTypeEnum,
                                      branch: newBranch,
                                      description: request.TenantDescription);

        try
        {
            await _repository.StartTransactionAsync(cancellationToken);

            var result = await _repository.InsertOneAsync(newTenant);

            if (result.IsFailure)
            {
                await _repository.RollbackTransaction(cancellationToken);

                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_repository.InsertOneAsync), "Failed to create tenant, transaction rolled back", new { tenantId = tenantId.ToString(), name = request.Name, subDomain = request.SubDomain, error = result.Error });

                return result.Error;
            }

            await _repository.CommitTransaction(cancellationToken);

            _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully created tenant", new { tenantId = tenantId.ToString(), name = request.Name, subDomain = request.SubDomain, tenantType = tenantTypeEnum.Name });

            return result.Response;
        }
        catch (Exception ex)
        {
            await _repository.RollbackTransaction(cancellationToken);

            var parameters = new { tenantId = tenantId.ToString(), name = request.Name, subDomain = request.SubDomain };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);

            throw;
        }
    }
}
