using Microsoft.Extensions.Hosting;
using Domain.Repositories;

namespace Infrastructure.Repositories;

/// <summary>
/// インストーラーファイルリポジトリ実装
/// </summary>
public class InstallerFileRepository(IHostEnvironment environment) : IInstallerFileRepository
{
    private const string InstallerFolder = "static_installers";
    private const string WindowsInstallerPattern = "*.exe";

    public async Task<Stream?> GetWindowsInstallerStreamAsync(CancellationToken cancellationToken = default)
    {
        var installerPath = GetWindowsInstallerPath();
        if (installerPath is null) return null;

        return await Task.FromResult<Stream>(new FileStream(installerPath, FileMode.Open, FileAccess.Read, FileShare.Read));
    }

    public string? GetWindowsInstallerFileName()
    {
        var installerPath = GetWindowsInstallerPath();
        return installerPath is null ? null : Path.GetFileName(installerPath);
    }

    private string? GetWindowsInstallerPath()
    {
        var installerDir = Path.Combine(environment.ContentRootPath, InstallerFolder);
        if (!Directory.Exists(installerDir)) return null;

        var installers = Directory.GetFiles(installerDir, WindowsInstallerPattern);
        return installers.Length > 0 ? installers[0] : null;
    }
}
