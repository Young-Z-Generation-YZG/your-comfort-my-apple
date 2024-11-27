
using Ardalis.SmartEnum;

namespace YGZ.Catalog.Domain.Core.Enums;

public class ProductStateEnum : SmartEnum<ProductStateEnum>
{
    public ProductStateEnum(string name, int value) : base(name, value) {}

    public static readonly ProductStateEnum ACTIVE = new(nameof(ACTIVE), 0);
    public static readonly ProductStateEnum INACTIVE = new(nameof(INACTIVE), 1);
    public static readonly ProductStateEnum DRAFT = new(nameof(DRAFT), 3);
    public static readonly ProductStateEnum DELETED = new(nameof(DELETED), 2);
}
