using Microsoft.Extensions.Logging;
using Quartz;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.ValueObjects;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Application.Orders.Jobs;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderHandler : ICommandHandler<CreateOrderCommand, bool>
{
    private readonly IGenericRepository<Order, OrderId> _repository;
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly ILogger<CreateOrderHandler> _logger;

    public CreateOrderHandler(IGenericRepository<Order, OrderId> repository,
                              ISchedulerFactory schedulerFactory,
                              ILogger<CreateOrderHandler> logger)
    {
        _repository = repository;
        _schedulerFactory = schedulerFactory;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var shippingAddress = ShippingAddress.Create(contactName: request.ShippingAddress.ContactName,
                                                     contactEmail: request.CustomerEmail,
                                                     contactPhoneNumber: request.ShippingAddress.ContactPhoneNumber,
                                                     addressLine: request.ShippingAddress.AddressLine,
                                                     district: request.ShippingAddress.District,
                                                     province: request.ShippingAddress.Province,
                                                     country: request.ShippingAddress.Country);

        var paymentMethod = EPaymentMethod.TryFromName(request.PaymentMethod, out var paymentMethodEnum);

        var newOrder = Order.Create(orderId: OrderId.Of(request.OrderId),
                                    tenantId: TenantId.Of(request.TenantId),
                                    branchId: BranchId.Of(request.BranchId),
                                    customerId: UserId.Of(request.CustomerId),
                                    customerPublicKey: request.CustomerPublicKey,
                                    tx: request.Tx,
                                    code: Code.GenerateCode(),
                                    paymentMethod: paymentMethodEnum,
                                    orderStatus: EOrderStatus.PENDING,
                                    shippingAddress: shippingAddress,
                                    promotionId: request.Promotion?.PromotionId,
                                    promotionType: request.Promotion?.PromotionType,
                                    discountType: request.Promotion?.DiscountType,
                                    discountValue: request.Promotion?.DiscountValue);

        if (paymentMethodEnum.Name == EPaymentMethod.SOLANA.Name)
        {
            newOrder.SetPaid();
        }

        foreach (var orderItem in request.OrderItems)
        {
            var newOrderItem = OrderItem.Create(orderItemId: OrderItemId.Create(),
                                                tenantId: TenantId.Of(request.TenantId),
                                                branchId: BranchId.Of(request.BranchId),
                                                orderId: newOrder.Id,
                                                skuId: orderItem.SkuId,
                                                modelId: orderItem.ModelId,
                                                modelName: orderItem.NormalizedModel,
                                                colorName: orderItem.NormalizedColor,
                                                storageName: orderItem.NormalizedStorage,
                                                unitPrice: orderItem.UnitPrice,
                                                displayImageUrl: orderItem.DisplayImageUrl,
                                                modelSlug: orderItem.ModelSlug,
                                                quantity: orderItem.Quantity,
                                                promotionId: orderItem.Promotion?.PromotionId,
                                                promotionType: orderItem.Promotion?.PromotionType,
                                                discountType: orderItem.Promotion?.DiscountType,
                                                discountValue: orderItem.Promotion?.DiscountValue,
                                                isReviewed: false);

            newOrder.AddOrderItem(newOrderItem);
        }


        var result = await _repository.AddAsync(newOrder, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        IScheduler scheduler = await _schedulerFactory.GetScheduler();

        var jobData = new JobDataMap
        {
            {"OrderId", newOrder.Id.Value.ToString() },
        };

        IJobDetail jobDetail = JobBuilder.Create<OrderAutoConfirmedJob>()
            .UsingJobData(jobData)
            .Build();

        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity($"trigger-auto-confirmed-order-{newOrder.Id.Value.ToString()}", "order-group")
            .ForJob(jobDetail)
            .StartAt(DateTimeOffset.UtcNow.AddMinutes(30)) // Start 30 minutes from now
            .WithSimpleSchedule(x => x.WithRepeatCount(0))
            .Build();

        await scheduler.ScheduleJob(jobDetail, trigger);

        return true;
    }
}
