using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.ServiceInterfaces;

namespace Api.Controllers;

/// <summary>
/// ダウンロードコントローラー
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DownloadController(IInstallerDownloadService downloadService) : ControllerBase
{
    private string? CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    [HttpGet("windows")]
    public async Task<IActionResult> DownloadWindowsInstaller(CancellationToken cancellationToken)
    {
        if (CurrentUserId is null) return Unauthorized();

        var result = await downloadService.GetWindowsInstallerAsync(CurrentUserId, cancellationToken);
        if (!result.IsSuccess)
        {
            return NotFound(result.ErrorMessages);
        }

        var (stream, fileName) = result.Value!;
        return File(stream, "application/octet-stream", fileName);
    }
}
