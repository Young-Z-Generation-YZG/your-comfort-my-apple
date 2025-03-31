
using Microsoft.EntityFrameworkCore;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Abstractions.Data;

public interface IUserRepository
{
    DbSet<User> GetDbSet();
    Task SaveChange();
}
