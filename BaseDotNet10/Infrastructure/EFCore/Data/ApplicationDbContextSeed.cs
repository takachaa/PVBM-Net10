using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Domain.Constants;

namespace Infrastructure.EFCore.Data;

/// <summary>
/// データベースシード
/// </summary>
public static class ApplicationDbContextSeed
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var logger = serviceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        string[] roles = [Roles.Admin, Roles.User];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                logger.LogInformation("ロール作成: {Role}", role);
            }
        }
    }
}
