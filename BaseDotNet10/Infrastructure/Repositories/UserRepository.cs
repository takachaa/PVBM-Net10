using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Repositories;

/// <summary>
/// ユーザーリポジトリ実装
/// </summary>
public class UserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
{
    public async Task<ApplicationUser?> GetByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await userManager.FindByIdAsync(userId);
    }

    public async Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<bool> CreateAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default)
    {
        var result = await userManager.CreateAsync(user, password);
        return result.Succeeded;
    }

    public async Task<bool> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> DeleteAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return false;

        var result = await userManager.DeleteAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        return await userManager.CheckPasswordAsync(user, password);
    }

    public async Task UpdateLastLoginAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is not null)
        {
            user.LastLoginAt = DateTime.UtcNow;
            await userManager.UpdateAsync(user);
        }
    }
}
