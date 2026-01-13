using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Infrastructure.EFCore.Data;

namespace Infrastructure.Extensions;

/// <summary>
/// マイグレーション拡張メソッド
/// </summary>
public static class MigrationExtensions
{
    /// <summary>
    /// マイグレーションとシードを適用
    /// </summary>
    public static async Task ApplyMigrationsAndSeedAsync(this IServiceProvider serviceProvider, ILogger logger)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            logger.LogInformation("データベースマイグレーションを適用中...");
            await context.Database.MigrateAsync();
            logger.LogInformation("マイグレーション完了");

            logger.LogInformation("シードデータを適用中...");
            await ApplicationDbContextSeed.SeedRolesAsync(scope.ServiceProvider);
            logger.LogInformation("シード完了");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "マイグレーションまたはシード中にエラーが発生しました");
            throw;
        }
    }
}
