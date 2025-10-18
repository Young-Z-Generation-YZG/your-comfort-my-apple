using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record IphoneSkuPriceResponse
{
    public required string Id { get; init; }
    public required string ModelId { get; init; }
    public required string UniqueQuery { get; init; }
    public required ModelResponse Model { get; init; }
    public required ColorResponse Color { get; init; }
    public required StorageResponse Storage { get; init; }
    public required decimal UnitPrice { get; init; }
}
