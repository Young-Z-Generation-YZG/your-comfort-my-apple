

using Microsoft.EntityFrameworkCore;
using System.Linq;
using YGZ.Ordering.Application.Core.Abstractions.Data;
using YGZ.Ordering.Application.Core.Abstractions.Messaging;
using YGZ.Ordering.Domain.Core.Abstractions.Result;
using YGZ.Ordering.Domain.Core.Errors;
using YGZ.Ordering.Domain.Orders;
using YGZ.Ordering.Domain.Orders.Entities;
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
        if(!Guid.TryParse(cmd.UserId, out Guid customerId))
        {
            return Errors.Customer.IdInvalid;
        };

        if(!OrderStatus.TryFromName(cmd.PaymentStatus, out OrderStatus status))
        {
            return Errors.Order.InvalidOrderStatus;
        }

        if (!PaymentType.TryFromName(cmd.PaymentType, out PaymentType paymentType))
        {
            return Errors.Order.InvalidPaymentType;
        }

        var shippingAddress = Address.CreateNew(cmd.ShippingAddress.ContactName,
                                                cmd.ShippingAddress.ContactEmail,
                                                cmd.ShippingAddress.ContactPhoneNumber,
                                                cmd.ShippingAddress.AddressLine,
                                                cmd.ShippingAddress.District,
                                                cmd.ShippingAddress.Province,
                                                cmd.ShippingAddress.Country);

        var billingAddress = Address.CreateNew(cmd.BillingAddress.ContactName,
                                                cmd.BillingAddress.ContactEmail,
                                                cmd.BillingAddress.ContactPhoneNumber,
                                                cmd.BillingAddress.AddressLine,
                                                cmd.BillingAddress.District,
                                                cmd.BillingAddress.Province,
                                                cmd.BillingAddress.Country);

        var newOrder = Order.CreateNew(CustomerId.Of(customerId), shippingAddress, billingAddress, status, paymentType);

        foreach (var orderLine in cmd.OrderLines)
        {
            try
            {
                var product = await _dbContext.Products
                    .FirstOrDefaultAsync(p => p.Id == ProductId.Of(orderLine.ProductTd), cancellationToken);

                if(product is null)
                {
                    var newProduct = Product.CreateNew(ProductId.Of(orderLine.ProductTd), orderLine.Model, orderLine.Color, orderLine.Storage, orderLine.Price);
                    _dbContext.Products.Add(newProduct);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            newOrder.AddOrderLine(ProductId.Of(orderLine.ProductTd),
                                  orderLine.Model,
                                  orderLine.Color,
                                  orderLine.Storage,
                                  orderLine.ProductImageUrl,
                                  orderLine.Quantity,
                                  orderLine.Price,
                                  orderLine.DiscountAmount,
                                  orderLine.SubTotal);
        }
        
        _dbContext.Orders.Add(newOrder);
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch(Exception ex)
        {
            return false;
        }

        return true;
    }
}
