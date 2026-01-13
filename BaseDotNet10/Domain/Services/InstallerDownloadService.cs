using Microsoft.Extensions.Logging;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Repositories;
using Domain.ServiceInterfaces;

namespace Domain.Services;

/// <summary>
/// インストーラーダウンロードサービス（C# 14: Primary Constructor）
/// </summary>
public class InstallerDownloadService(
    IInstallerFileRepository installerFileRepository,
    IInstallHistoryRepository installHistoryRepository,
    ILogger<InstallerDownloadService> logger) : IInstallerDownloadService
{
    public async Task<Result<(Stream Stream, string FileName)>> GetWindowsInstallerAsync(string userId, CancellationToken cancellationToken = default)
    {
        var stream = await installerFileRepository.GetWindowsInstallerStreamAsync(cancellationToken);
        var fileName = installerFileRepository.GetWindowsInstallerFileName();

        if (stream is null || fileName is null)
        {
            logger.LogWarning("Windowsインストーラーが見つかりません");
            return Result<(Stream, string)>.Failure("インストーラーが見つかりません");
        }

        // ダウンロード履歴を記録
        var history = new InstallHistory
        {
            UserId = userId,
            OS = "Windows",
            InstalledAt = DateTime.UtcNow
        };
        await installHistoryRepository.AddAsync(history, cancellationToken);

        logger.LogInformation("Windowsインストーラーダウンロード: {UserId}", userId);
        return Result<(Stream, string)>.Success((stream, fileName));
    }
}
