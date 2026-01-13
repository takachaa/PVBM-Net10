using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.EFCore.Data;

namespace Infrastructure.Repositories;

/// <summary>
/// 2要素認証リポジトリ実装
/// </summary>
public class TwoFactorAuthRepository(ApplicationDbContext context) : ITwoFactorAuthRepository
{
    public async Task<string> GenerateAndSaveCodeAsync(string userId, CancellationToken cancellationToken = default)
    {
        var code = Random.Shared.Next(100000, 999999).ToString();

        var twoFactorAuth = new TwoFactorAuth
        {
            UserId = userId,
            Code = code,
            ExpirationTime = DateTime.UtcNow.AddMinutes(10),
            IsUsed = false
        };

        context.TwoFactorAuths.Add(twoFactorAuth);
        await context.SaveChangesAsync(cancellationToken);

        return code;
    }

    public async Task<TwoFactorAuth?> GetValidCodeAsync(string userId, string code, CancellationToken cancellationToken = default)
    {
        return await context.TwoFactorAuths
            .FirstOrDefaultAsync(t =>
                t.UserId == userId &&
                t.Code == code &&
                !t.IsUsed &&
                t.ExpirationTime > DateTime.UtcNow,
                cancellationToken);
    }

    public async Task MarkCodeAsUsedAsync(string userId, string code, CancellationToken cancellationToken = default)
    {
        var twoFactorAuth = await context.TwoFactorAuths
            .FirstOrDefaultAsync(t => t.UserId == userId && t.Code == code, cancellationToken);

        if (twoFactorAuth is not null)
        {
            twoFactorAuth.IsUsed = true;
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeleteExpiredCodesAsync(CancellationToken cancellationToken = default)
    {
        var expiredCodes = await context.TwoFactorAuths
            .Where(t => t.ExpirationTime < DateTime.UtcNow || t.IsUsed)
            .ToListAsync(cancellationToken);

        context.TwoFactorAuths.RemoveRange(expiredCodes);
        await context.SaveChangesAsync(cancellationToken);
    }
}
