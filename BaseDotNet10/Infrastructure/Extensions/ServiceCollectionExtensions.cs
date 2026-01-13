using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Entities;
using Domain.Repositories;
using Domain.ServiceInterfaces;
using Domain.Services;
using Infrastructure.EFCore.Data;
using Infrastructure.Repositories;

namespace Infrastructure.Extensions;

/// <summary>
/// サービスコレクション拡張メソッド
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Infrastructure層のサービスを登録
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Identity
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            // パスワード要件
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;

            // メール
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;

            // ロックアウト
            options.Lockout.MaxFailedAccessAttempts = 10;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.AllowedForNewUsers = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITwoFactorAuthRepository, TwoFactorAuthRepository>();
        services.AddScoped<IEmailRepository, SendGridEmailRepository>();
        services.AddScoped<IInstallerFileRepository, InstallerFileRepository>();
        services.AddScoped<IInstallHistoryRepository, InstallHistoryRepository>();

        return services;
    }

    /// <summary>
    /// Domain層のサービスを登録
    /// </summary>
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IInstallerDownloadService, InstallerDownloadService>();
        services.AddScoped<IContactService, ContactService>();

        return services;
    }

    /// <summary>
    /// Cookie認証を設定
    /// </summary>
    public static IServiceCollection AddCookieAuthentication(this IServiceCollection services)
    {
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "App.Session";
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            options.SlidingExpiration = true;
            options.LoginPath = "/api/auth/login";
            options.LogoutPath = "/api/auth/logout";

            // API用: リダイレクトではなくステータスコードを返す
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            };
            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = 403;
                return Task.CompletedTask;
            };
        });

        return services;
    }
}
