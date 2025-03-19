

namespace YGZ.Catalog.Application.Common.Commands;

public sealed record IPhoneModelCommand
{
    public string ModelName { get; set; } = default!;
    public int? ModelOrder { get; set; } = default!;
}
