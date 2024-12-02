

using YGZ.Ordering.Application.Core.Abstractions.Data;
using YGZ.Ordering.Application.Core.Abstractions.Messaging;
using YGZ.Ordering.Domain.Core.Abstractions.Result;
using YGZ.Ordering.Domain.Core.Errors;
using YGZ.Ordering.Domain.Orders;
using YGZ.Ordering.Domain.Orders.ValueObjects;
using static YGZ.Ordering.Domain.Core.Enums.Enums;

namespace YGZ.Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, bool>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateOrderCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<bool>> Handle(CreateOrderCommand cmd, CancellationToken cancellationToken)
    {
        if(!Guid.TryParse(cmd.User_id, out Guid customerId))
        {
            return Errors.Customer.IdInvalid;
        };

        if(!OrderStatus.TryFromName(cmd.Payment_status, out OrderStatus status))
        {
            return Errors.Order.InvalidOrderStatus;
        }

        if (!PaymentType.TryFromName(cmd.Payment_type, out PaymentType paymentType))
        {
            return Errors.Order.InvalidPaymentType;
        }

        var shippingAddress = Address.CreateNew(null,
                                                    null,
                                                    null,
                                                    null,
                                                    null,
                                                    null,
                                                    null);
        var billingAddress = Address.CreateNew(null,
                                                    null,
                                                    null,
                                                    null,
                                                    null,
                                                    null,
                                                    null);

        //if (cmd.Shipping_address is not null)
        //{
        //    shippingAddress = Address.CreateNew(cmd.Shipping_address.Contact_name,
        //                                            cmd.Shipping_address.Contact_email,
        //                                            cmd.Shipping_address.Contact_phone_number,
        //                                            cmd.Shipping_address.Address_line,
        //                                            cmd.Shipping_address.District,
        //                                            cmd.Shipping_address.Province,
        //                                            cmd.Shipping_address.Country);
        //}

        //if(cmd.Billing_address is not null)
        //{
        //    billingAddress = Address.CreateNew(cmd.Billing_address.Contact_name,
        //                                           cmd.Billing_address.Contact_email,
        //                                           cmd.Billing_address.Contact_phone_number,
        //                                           cmd.Billing_address.Address_line,
        //                                           cmd.Billing_address.District,
        //                                           cmd.Billing_address.Province,
        //                                           cmd.Billing_address.Country);
        //}

        var newOrder = Order.CreateNew(CustomerId.Of(customerId), shippingAddress, billingAddress, status, paymentType);

        _dbContext.Orders.Add(newOrder);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
