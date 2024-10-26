
using YGZ.Catalog.Domain.Core.Abstractions.Common;

namespace YGZ.Catalog.Infrastructure.Common;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
}
