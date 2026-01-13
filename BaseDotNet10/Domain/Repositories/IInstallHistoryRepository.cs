using Domain.Entities;

namespace Domain.Repositories;

/// <summary>
/// インストール履歴リポジトリインターフェース
/// </summary>
public interface IInstallHistoryRepository
{
    Task<IReadOnlyList<InstallHistory>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task AddAsync(InstallHistory history, CancellationToken cancellationToken = default);
}
