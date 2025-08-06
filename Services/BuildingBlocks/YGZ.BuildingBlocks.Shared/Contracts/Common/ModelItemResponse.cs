

using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Common;

[JsonConverter(typeof(SnakeCaseSerializer))]
public class ModelItemResponse
{
    required public string ModelName { get; set; }
    required public int ModelOrder { get; set; }
}
