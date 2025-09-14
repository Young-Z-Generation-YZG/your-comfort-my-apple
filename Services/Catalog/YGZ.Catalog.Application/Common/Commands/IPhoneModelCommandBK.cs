

namespace YGZ.Catalog.Application.Common.Commands;

public sealed record IPhoneModelCommandBK
{
    public string ModelName { get; set; } = default!;
    public int? ModelOrder { get; set; } = default!;
}
