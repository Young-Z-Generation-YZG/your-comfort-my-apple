
namespace YGZ.Ordering.Application.Common.Commands;

public record AddressCommand(string Contact_name,
                             string Contact_email,
                             string Contact_phone_number,
                             string Address_line,
                             string District,
                             string Province,
                             string Country) { }
