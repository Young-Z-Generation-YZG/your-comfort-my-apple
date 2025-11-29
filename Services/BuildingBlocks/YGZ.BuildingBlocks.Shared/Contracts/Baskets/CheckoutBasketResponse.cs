

using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Baskets;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record CheckoutBasketResponse()
{
    public required string OrderId { get; init; }
    public required List<CartItemResponse> CartItems { get; init; }
    public string? PaymentRedirectUrl { get; init; }
    public string? OrderDetailsRedirectUrl { get; init; }
}
