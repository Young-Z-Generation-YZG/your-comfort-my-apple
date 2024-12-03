using Swashbuckle.AspNetCore.Filters;
using YGZ.Ordering.Application.Common.Commands;
using YGZ.Ordering.Application.Orders.Commands.CreateOrder;

namespace YGZ.Ordering.Api.Common.SwaggerExamples;

public class CreateOrderExample : IExamplesProvider<CreateOrderCommand>
{
    public CreateOrderCommand GetExamples()
    {
        var shippingAddress = new AddressCommand
        (
            "John Doe",
            "lov3rinve146@gmail.com",
            "123456789",
            "1234 Main St",
            "District 1",
            "Province 1",
            "Country 1"
        );

        var billingAddress = new AddressCommand
        (
            "John Doe",
            "lov3rinve146@gmail.com",
            "123456789",
            "1234 Main St",
            "District 1",
            "Province 1",
            "Country 1"
        );

        var orderLines = new List<OrderLineCommand>
        {
            new("674e8be4ec6966234e16b846", "iPhone 16", "pink", 128, "slug", 799, 1, 799)
        };

        return new CreateOrderCommand("672cdaed4e67692dff64a47c", shippingAddress, billingAddress, "PENDING", "MOMO", orderLines);
    }
}
