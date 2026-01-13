using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.EFCore.Data;

namespace Infrastructure.Repositories;

/// <summary>
/// インストール履歴リポジトリ実装
/// </summary>
public class InstallHistoryRepository(ApplicationDbContext context) : IInstallHistoryRepository
{
    public async Task<IReadOnlyList<InstallHistory>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.InstallHistories
            .Where(h => h.UserId == userId)
            .OrderByDescending(h => h.InstalledAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(InstallHistory history, CancellationToken cancellationToken = default)
    {
        context.InstallHistories.Add(history);
        await context.SaveChangesAsync(cancellationToken);
    }
}
