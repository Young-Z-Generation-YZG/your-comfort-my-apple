
using Ardalis.SmartEnum;

namespace YGZ.Catalog.Domain.Core.Enums;

public class EStorage : SmartEnum<EStorage>
{
    public EStorage(string name, int value) : base(name, value) { }

    public static readonly EStorage STORAGE_64 = new("64GB", 64);
    public static readonly EStorage STORAGE_128 = new("128GB", 128);
    public static readonly EStorage STORAGE_256 = new("256GB", 256);
    public static readonly EStorage STORAGE_512 = new("512GB", 512);
    public static readonly EStorage STORAGE_1024 = new("1TB", 1024);
}
