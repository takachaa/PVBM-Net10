using Domain.Abstractions;

namespace Domain.ServiceInterfaces;

/// <summary>
/// インストーラーダウンロードサービスインターフェース
/// </summary>
public interface IInstallerDownloadService
{
    Task<Result<(Stream Stream, string FileName)>> GetWindowsInstallerAsync(string userId, CancellationToken cancellationToken = default);
}
