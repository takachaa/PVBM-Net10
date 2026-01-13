using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Dtos;
using Domain.ServiceInterfaces;

namespace Api.Controllers;

/// <summary>
/// 認証コントローラー
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthenticationService authService) : ControllerBase
{
    private string? CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request, CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessages);
    }

    [HttpPost("register/admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequestDto request, CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAdminAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessages);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Unauthorized(result.ErrorMessages);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var result = await authService.LogoutAsync(cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.ErrorMessages);
    }

    [HttpPost("send-2fa-code")]
    public async Task<IActionResult> SendTwoFactorCode([FromBody] TwoFactorCodeRequestDto request, CancellationToken cancellationToken)
    {
        var result = await authService.GenerateAndSendTwoFactorCodeAsync(request.UserId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessages);
    }

    [HttpPost("verify-2fa-code")]
    public async Task<IActionResult> VerifyTwoFactorCode([FromBody] VerifyCodeRequestDto request, CancellationToken cancellationToken)
    {
        var result = await authService.ValidateTwoFactorCodeAsync(request.UserId, request.Code, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessages);
    }

    [HttpPost("login/2fa")]
    public async Task<IActionResult> LoginWithTwoFactor([FromBody] VerifyCodeRequestDto request, CancellationToken cancellationToken)
    {
        var result = await authService.LoginWithTwoFactorAsync(request.UserId, request.Code, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Unauthorized(result.ErrorMessages);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request, CancellationToken cancellationToken)
    {
        var result = await authService.ForgotPasswordAsync(request, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.ErrorMessages);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request, CancellationToken cancellationToken)
    {
        var result = await authService.ResetPasswordAsync(request, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.ErrorMessages);
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string code, CancellationToken cancellationToken)
    {
        var result = await authService.ConfirmEmailAsync(userId, code, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.ErrorMessages);
    }

    [HttpPost("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequestDto request, CancellationToken cancellationToken)
    {
        var result = await authService.ResendConfirmationEmailAsync(request, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.ErrorMessages);
    }

    [HttpGet("manage/info")]
    [Authorize]
    public async Task<IActionResult> GetUserInfo(CancellationToken cancellationToken)
    {
        if (CurrentUserId is null) return Unauthorized();

        var result = await authService.GetUserInfoAsync(CurrentUserId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.ErrorMessages);
    }

    [HttpGet("mypage")]
    [Authorize]
    public async Task<IActionResult> GetMyPage(CancellationToken cancellationToken)
    {
        if (CurrentUserId is null) return Unauthorized();

        var result = await authService.GetUserProfileAsync(CurrentUserId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.ErrorMessages);
    }

    [HttpPut("mypage")]
    [Authorize]
    public async Task<IActionResult> UpdateMyPage([FromBody] UpdateProfileRequestDto request, CancellationToken cancellationToken)
    {
        if (CurrentUserId is null) return Unauthorized();

        var result = await authService.UpdateProfileAsync(CurrentUserId, request, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.ErrorMessages);
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request, CancellationToken cancellationToken)
    {
        if (CurrentUserId is null) return Unauthorized();

        var result = await authService.ChangePasswordAsync(CurrentUserId, request, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.ErrorMessages);
    }
}
