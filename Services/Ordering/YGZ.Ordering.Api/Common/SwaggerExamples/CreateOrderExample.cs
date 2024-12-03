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
            new("674e8be4ec6966234e16b846",
                "iPhone 16",
                "pink",
                128,
                "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-7inch-ultramarine?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4OUp1NDJCalJ6MnpHSm1KdCtRZ0FvSDJrQmVLSXFrTCsvY1VvVmRlZkVnMzJKTG1lVWJJT2RXQWE0Mm9rU1V0V0R6SkNnaG1kYkl1VUVsNXVsVGJrQ0s0UmdXWi9jaTBCeEx5VFNDNXdWbmdB&traceId=1",
                799,
                1,
                168,
                799)
        };

        return new CreateOrderCommand("672cdaed4e67692dff64a47c", shippingAddress, billingAddress, "PENDING", "MOMO", orderLines);
    }
}
