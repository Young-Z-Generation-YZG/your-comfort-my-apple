
namespace YGZ.BuildingBlocks.Shared.Contracts.Baskets;

public record CartLineRequest(
                            string ProductItemId,
                            string Model,
                            string Color,
                            int Storage,
                            string Primary_image_url,
                            decimal Price,
                            int Quantity) { }
