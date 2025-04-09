

using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Baskets;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record CheckoutBasketResponse()
{
    required public string PaymentRedirectUrl { get; set; }
}
