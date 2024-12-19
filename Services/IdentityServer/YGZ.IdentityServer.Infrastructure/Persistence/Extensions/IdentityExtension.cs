

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.IdentityServer.Domain.Users;
using YGZ.IdentityServer.Infrastructure.Persistence.Data;

namespace YGZ.IdentityServer.Infrastructure.Persistence.Extensions;

public static class IdentityExtension
{
    public static IServiceCollection AddIdentityExtension(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStrings = configuration.GetConnectionString(ConnectionStrings.IdentityDb!);

        services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

        //services.AddDefaultIdentity<User>()
        //        .AddEntityFrameworkStores<ApplicationDbContext>()
        //        .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options => {
            // Thiết lập về Password
            options.Password.RequireDigit = false; // Không bắt phải có số
            options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
            options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
            options.Password.RequireUppercase = false; // Không bắt buộc chữ in
            options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
            options.Password.RequiredUniqueChars = 0; // Số ký tự riêng biệt

            // Cấu hình về User.
            options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;  // Email là duy nhất

            // Cấu hình đăng nhập.
            options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
            options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
        });

        return services;
    }
}
