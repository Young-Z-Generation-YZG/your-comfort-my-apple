
namespace YGZ.Ordering.Application.Common.Commands;

public record AddressCommand(string ContactName,
                             string ContactEmail,
                             string ContactPhoneNumber,
                             string AddressLine,
                             string District,
                             string Province,
                             string Country) { }
