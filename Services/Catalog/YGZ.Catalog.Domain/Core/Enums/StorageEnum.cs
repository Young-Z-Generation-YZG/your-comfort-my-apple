

using Ardalis.SmartEnum;

namespace YGZ.Catalog.Domain.Core.Enums;

public class StorageEnum : SmartEnum<StorageEnum>
{
    public StorageEnum(string name, int value) : base(name, value) { }

    public static readonly StorageEnum STORAGE_64 = new("64GB", 64);
    public static readonly StorageEnum STORAGE_128 = new("128GB", 128);
    public static readonly StorageEnum STORAGE_256 = new("256GB", 256);
    public static readonly StorageEnum STORAGE_512 = new("512GB", 512);
    public static readonly StorageEnum STORAGE_1024 = new("1TB", 1024);
    public static readonly StorageEnum STORAGE_2048 = new("2TB", 2048);

    public static new StorageEnum FromValue(int value)
    {
        return SmartEnum<StorageEnum, int>.FromValue(value);
    }
}
