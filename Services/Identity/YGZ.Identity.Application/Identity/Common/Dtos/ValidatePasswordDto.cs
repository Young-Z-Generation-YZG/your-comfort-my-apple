
using YGZ.Identity.Domain.Identity.Entities;

namespace YGZ.Identity.Application.Identity.Common.Dtos;

public sealed record ValidatePasswordDto(User User, string HashPassword, string RequestPassword) { }
