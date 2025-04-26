using Microsoft.Extensions.Logging;
using Quartz;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Application.Orders.Commands.CreateOrder.Extensions;
using YGZ.Ordering.Application.Orders.Jobs;

namespace YGZ.Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly ILogger<CreateOrderCommandHandler> _logger;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, ILogger<CreateOrderCommandHandler> logger, ISchedulerFactory schedulerFactory)
    {
        _orderRepository = orderRepository;
        _logger = logger;
        _schedulerFactory = schedulerFactory;
    }

    public async Task<Result<bool>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = request.ToEntity(request.OrderId, request.CustomerId, request.CustomerEmail);

        var result = await _orderRepository.AddAsync(order, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        IScheduler scheduler = await _schedulerFactory.GetScheduler();

        var jobData = new JobDataMap
        {
            {"OrderId", order.Id.Value.ToString() },
        };

        IJobDetail jobDetail = JobBuilder.Create<OrderAutoConfirmedJob>()
            .UsingJobData(jobData)
            .Build();

        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity($"trigger-auto-confirmed-order-{order.Id.Value.ToString()}", "order-group")
            .ForJob(jobDetail)
            .StartAt(DateTimeOffset.UtcNow.AddMinutes(30)) // Start 30 minutes from now
            .WithSimpleSchedule(x => x.WithRepeatCount(0))
            .Build();

        await scheduler.ScheduleJob(jobDetail, trigger);

        return true;
    }
}
