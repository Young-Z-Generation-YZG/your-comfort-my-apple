using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class ESkuRequestState : SmartEnum<ESkuRequestState>
{
    public ESkuRequestState(string name, int value) : base(name, value) { }

    public static readonly ESkuRequestState PENDING = new(nameof(PENDING), 0);
    public static readonly ESkuRequestState APPROVED = new(nameof(APPROVED), 0);
    public static readonly ESkuRequestState REJECTED = new(nameof(REJECTED), 0);
}
