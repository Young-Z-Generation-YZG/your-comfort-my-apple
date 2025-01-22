

//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using YGZ.IdentityServer.Domain.Users;
//using YGZ.IdentityServer.Infrastructure.Persistence.Data;
//using YGZ.IdentityServer.Infrastructure.Settings;

//namespace YGZ.IdentityServer.Infrastructure.Extensions.IdentityServer;

//public static class IdentityExtension
//{
//    public static IServiceCollection AddIdentityExtension(this IServiceCollection services, IConfiguration configuration)
//    {
//        var connectionStrings = configuration.GetConnectionString(ConnectionStrings.IdentityDb!);
//        var identityServerSettings = configuration.GetSection(IdentityServerSettings.SettingKey!).Get<IdentityServerSettings>()!;

//        services.AddIdentity<User, IdentityRole>()
//                .AddEntityFrameworkStores<ApplicationDbContext>()
//                .AddDefaultTokenProviders();

//        services.Configure<IdentityOptions>(options =>
//        {
//            // Thiết lập về Password
//            options.Password.RequireDigit = true; // Không bắt phải có số
//            options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
//            options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
//            options.Password.RequireUppercase = false; // Không bắt buộc chữ in
//            options.Password.RequiredLength = 6; // Số ký tự tối thiểu của password
//            options.Password.RequiredUniqueChars = 0; // Số ký tự riêng biệt

//            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);  // Khóa 1 phút
//            options.Lockout.MaxFailedAccessAttempts = 5;

//            // Cấu hình về User.
//            options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
//                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
//            options.User.RequireUniqueEmail = true;  // Email là duy nhất

//            // Cấu hình đăng nhập.
//            options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
//            options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
//        });


//        // Config Google Authentication
//        services.AddAuthentication()
//                .AddGoogle(options =>
//                {
//                    options.ClientId = identityServerSettings.ExternalOauthSettings.GoogleOauthSettings.ClientId;
//                    options.ClientSecret = identityServerSettings.ExternalOauthSettings.GoogleOauthSettings.ClientSecret;
//                    //options.CallbackPath = ""; // default: https://localhost:5055/signin-google
//                    options.CallbackPath = identityServerSettings.ExternalOauthSettings.GoogleOauthSettings.CallbackPath;
//                });

//        return services;
//    }
//}
