using System.Text.Json.Serialization;

namespace YGZ.Ordering.Api.Contracts;

public sealed record UpdateOrderStatusRequest()
{
    required public string _updateStatus { get; set; }
}
