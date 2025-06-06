﻿
using Ardalis.SmartEnum;

namespace YGZ.Catalog.Domain.Core.Enums;

public class DiscountState : SmartEnum<DiscountState>
{
    public DiscountState(string name, int value) : base(name, value) { }

    private DiscountState() : base("INACTIVE", 1) { }

    public static readonly DiscountState UNKNOWN = new("UNKNOWN", 0);
    public static readonly DiscountState ACTIVE = new("ACTIVE", 1);
    public static readonly DiscountState INACTIVE = new("INACTIVE", 2);
    public static readonly DiscountState EXPIRED = new("EXPIRED", 3);
}
