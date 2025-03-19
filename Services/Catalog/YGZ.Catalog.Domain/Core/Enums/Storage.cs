


using Ardalis.SmartEnum;

namespace YGZ.Catalog.Domain.Core.Enums;

public class Storage : SmartEnum<Storage>
{
    public Storage(string name, int value) : base(name, value) { }

    public static readonly Storage STORAGE_64 = new("64GB", 64);
    public static readonly Storage STORAGE_128 = new("128GB", 128);
    public static readonly Storage STORAGE_256 = new("256GB", 256);
    public static readonly Storage STORAGE_512 = new("512GB", 512);
    public static readonly Storage STORAGE_1024 = new("1TB", 1024);

    public static new Storage FromValue(int value)
    {
        return SmartEnum<Storage, int>.FromValue(value);
    }
}
