using Domain.Entities;

namespace Domain.Repositories;

/// <summary>
/// ユーザーリポジトリインターフェース
/// </summary>
public interface IUserRepository
{
    Task<ApplicationUser?> GetByIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> CreateAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task UpdateLastLoginAsync(string userId, CancellationToken cancellationToken = default);
}
