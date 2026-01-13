using Domain.Entities;

namespace Domain.Repositories;

/// <summary>
/// 2要素認証リポジトリインターフェース
/// </summary>
public interface ITwoFactorAuthRepository
{
    Task<string> GenerateAndSaveCodeAsync(string userId, CancellationToken cancellationToken = default);
    Task<TwoFactorAuth?> GetValidCodeAsync(string userId, string code, CancellationToken cancellationToken = default);
    Task MarkCodeAsUsedAsync(string userId, string code, CancellationToken cancellationToken = default);
    Task DeleteExpiredCodesAsync(CancellationToken cancellationToken = default);
}
