using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Requests.SkuRequest.Events;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Requests.Events.DomainEvents;

public class SkuRequestApprovedDomainEventHandler : INotificationHandler<SkuRequestApprovedDomainEvent>
{
    private readonly IMongoRepository<SKU, SkuId> _skuRepository;
    private readonly ILogger<SkuRequestApprovedDomainEventHandler> _logger;
    public SkuRequestApprovedDomainEventHandler(IMongoRepository<SKU, SkuId> skuRepository, ILogger<SkuRequestApprovedDomainEventHandler> logger)
    {
        _skuRepository = skuRepository;
        _logger = logger;
    }

    public async Task Handle(SkuRequestApprovedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("::::[DomainEventHandler:{DomainEventHandler}]:::: Processing SKU request approved domain event. SKU ID: {SkuId}",
            nameof(SkuRequestApprovedDomainEventHandler), notification.SkuRequest.Sku.SkuId.Value);

        var filters = Builders<SKU>.Filter.And(
            Builders<SKU>.Filter.Eq("branch_id", new ObjectId(notification.SkuRequest.ToBranch.BranchId.Value)),
            Builders<SKU>.Filter.Eq(x => x.Model.NormalizedName, notification.SkuRequest.Sku.ModelNormalizedName),
            Builders<SKU>.Filter.Eq(x => x.Color.NormalizedName, notification.SkuRequest.Sku.ColorNormalizedName),
            Builders<SKU>.Filter.Eq(x => x.Storage.NormalizedName, notification.SkuRequest.Sku.StorageNormalizedName)
        );


        var skuRequestToTenant = await _skuRepository.GetByFilterAsync(filters, cancellationToken);

        if (skuRequestToTenant is null)
        {
            _logger.LogError("::::[DomainEventHandler:{DomainEventHandler}][Result:Error]:::: SKU not found. SKU ID: {SkuId}",
                nameof(SkuRequestApprovedDomainEventHandler), notification.SkuRequest.Sku.SkuId.Value);
            return;
        }

        try
        {
            skuRequestToTenant.SetReservedForSkuRequest(
                toBranchId: notification.SkuRequest.FromBranch.BranchId,
                toBranchName: notification.SkuRequest.FromBranch.BranchName,
                reservedQuantity: notification.SkuRequest.RequestQuantity
            );
        }
        catch (Exception ex)
        {
            _logger.LogError("::::[DomainEventHandler:{DomainEventHandler}][Result:Error]:::: Failed to set reserved for SKU request. SKU ID: {SkuId}, Error: {ErrorMessage}",
                nameof(SkuRequestApprovedDomainEventHandler), notification.SkuRequest.Sku.SkuId.Value, ex.Message);
            return;
        }

        await _skuRepository.UpdateAsync(skuRequestToTenant.Id.Value!, skuRequestToTenant);

        _logger.LogInformation("::::[DomainEventHandler:{DomainEventHandler}]:::: Successfully processed SKU request approved domain event. SKU ID: {SkuId}",
            nameof(SkuRequestApprovedDomainEventHandler), notification.SkuRequest.Sku.SkuId.Value);
    }
}
