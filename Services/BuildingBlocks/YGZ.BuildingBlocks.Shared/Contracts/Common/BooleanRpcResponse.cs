

namespace YGZ.BuildingBlocks.Shared.Contracts.Common;

public record BooleanRpcResponse
{
    public bool IsSuccess { get; set; } = false;
    public bool IsFailure => !IsSuccess;
    public string? ErrorMessage { get; set; } = null;
}