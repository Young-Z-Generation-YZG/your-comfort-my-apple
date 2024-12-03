using Swashbuckle.AspNetCore.Filters;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;

namespace YGZ.Basket.Api.Common.SwaggerExamples;

public class StoreBasketRequestExample : IExamplesProvider<StoreBasketRequest>
{
    public StoreBasketRequest GetExamples()
    {
        return new StoreBasketRequest("78dc45ca-a007-4d33-9616-2d8e44735e1a", new List<CartLineRequest>
        {
            new CartLineRequest("674f20b3ed034b761cd47ec2",
                                "iPhone 16",
                                "ultramarine",
                                128,
                                "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-7inch-ultramarine?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4OUp1NDJCalJ6MnpHSm1KdCtRZ0FvSDJrQmVLSXFrTCsvY1VvVmRlZkVnMzJKTG1lVWJJT2RXQWE0Mm9rU1V0V0R6SkNnaG1kYkl1VUVsNXVsVGJrQ0s0UmdXWi9jaTBCeEx5VFNDNXdWbmdB&traceId=1",
                                799,
                                1),
        });
    }
}
