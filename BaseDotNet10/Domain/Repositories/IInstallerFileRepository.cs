namespace Domain.Repositories;

/// <summary>
/// インストーラーファイルリポジトリインターフェース
/// </summary>
public interface IInstallerFileRepository
{
    Task<Stream?> GetWindowsInstallerStreamAsync(CancellationToken cancellationToken = default);
    string? GetWindowsInstallerFileName();
}
