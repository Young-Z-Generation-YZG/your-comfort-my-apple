using Swashbuckle.AspNetCore.Filters;
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
            new("672cdaed4e67692dff64a47c", "Product 1", "Model 1", "Color 1", 64, "product-1", 100, 1),
            new("672cdaed4e67692dff64a47c", "Product 2", "Model 2", "Color 2", 128, "product-2", 200, 2),
            new("672cdaed4e67692dff64a47c", "Product 3", "Model 3", "Color 3", 256, "product-3", 300, 3)
        };

        return new CreateOrderCommand("672cdaed4e67692dff64a47c", shippingAddress, billingAddress, "PENDING", "MOMO", orderLines);
    }
}
