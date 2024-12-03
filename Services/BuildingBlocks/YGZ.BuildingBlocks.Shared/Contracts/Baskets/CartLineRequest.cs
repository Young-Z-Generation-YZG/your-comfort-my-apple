
namespace YGZ.BuildingBlocks.Shared.Contracts.Baskets;

public record CartLineRequest(
                            string ProductItemId,
                            string Model,
                            string Color,
                            int Storage,
                            decimal Price,
                            int Quantity) { }
