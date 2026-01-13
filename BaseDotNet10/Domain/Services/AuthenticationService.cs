using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using Domain.Abstractions;
using Domain.Constants;
using Domain.Dtos;
using Domain.Entities;
using Domain.Repositories;
using Domain.ServiceInterfaces;

namespace Domain.Services;

/// <summary>
/// 認証サービス実装（C# 14: Primary Constructor）
/// </summary>
public class AuthenticationService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ITwoFactorAuthRepository twoFactorAuthRepository,
    IEmailRepository emailRepository,
    IInstallHistoryRepository installHistoryRepository,
    ILogger<AuthenticationService> logger,
    IConfiguration configuration) : IAuthenticationService
{
    public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("ユーザー登録開始: {Email}", request.Email);

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            OrganizationName = request.OrganizationName ?? string.Empty,
            CreatedAt = DateTime.UtcNow
        };

        var createResult = await userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            var errors = createResult.Errors.Select(e => e.Description);
            logger.LogError("ユーザー作成失敗: {Email}", request.Email);
            return Result<AuthResponseDto>.Failure(errors);
        }

        var roleResult = await userManager.AddToRoleAsync(user, Roles.User);
        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);
            return Result<AuthResponseDto>.Failure("ユーザーロールの割り当てに失敗しました");
        }

        // メール確認リンク送信
        try
        {
            var confirmationLink = await GenerateEmailConfirmationLinkAsync(user);
            await emailRepository.SendEmailConfirmationAsync(user.Email!, confirmationLink, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "確認メール送信失敗: {Email}", user.Email);
        }

        logger.LogInformation("ユーザー作成成功: {Email}", request.Email);
        return Result<AuthResponseDto>.Success(new AuthResponseDto(true, DomainConstants.Messages.RegistrationSuccess, UserId: user.Id));
    }

    public async Task<Result<AuthResponseDto>> RegisterAdminAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("管理者登録開始: {Email}", request.Email);

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            OrganizationName = request.OrganizationName ?? string.Empty,
            CreatedAt = DateTime.UtcNow
        };

        var createResult = await userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            return Result<AuthResponseDto>.Failure(createResult.Errors.Select(e => e.Description));
        }

        var roleResult = await userManager.AddToRoleAsync(user, Roles.Admin);
        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);
            return Result<AuthResponseDto>.Failure("管理者ロールの割り当てに失敗しました");
        }

        logger.LogInformation("管理者作成成功: {Email}", request.Email);
        return Result<AuthResponseDto>.Success(new AuthResponseDto(true, UserId: user.Id));
    }

    public async Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("ログイン試行: {Email}", request.Email);

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result<AuthResponseDto>.Failure(DomainConstants.Errors.UserNotFound);
        }

        var signInResult = await signInManager.PasswordSignInAsync(user, request.Password, isPersistent: true, lockoutOnFailure: true);

        if (signInResult.IsLockedOut)
        {
            return Result<AuthResponseDto>.Failure(DomainConstants.Errors.AccountLocked);
        }

        if (!signInResult.Succeeded)
        {
            return Result<AuthResponseDto>.Failure(DomainConstants.Errors.InvalidCredentials);
        }

        user.LastLoginAt = DateTime.UtcNow;
        await userManager.UpdateAsync(user);

        logger.LogInformation("ログイン成功: {Email}", request.Email);
        return Result<AuthResponseDto>.Success(new AuthResponseDto(true, DomainConstants.Messages.LoginSuccess, UserId: user.Id));
    }

    public async Task<Result> LogoutAsync(CancellationToken cancellationToken = default)
    {
        await signInManager.SignOutAsync();
        logger.LogInformation("ログアウト成功");
        return Result.Success();
    }

    public async Task<Result<TwoFactorAuthResponseDto>> GenerateAndSendTwoFactorCodeAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user?.Email is null)
        {
            return Result<TwoFactorAuthResponseDto>.Failure(DomainConstants.Errors.UserNotFound);
        }

        var code = await twoFactorAuthRepository.GenerateAndSaveCodeAsync(userId, cancellationToken);
        await emailRepository.SendAuthenticationCodeAsync(user.Email, code, cancellationToken);

        logger.LogInformation("2FA コード送信: {Email}", user.Email);
        return Result<TwoFactorAuthResponseDto>.Success(new TwoFactorAuthResponseDto(true, DomainConstants.Messages.TwoFactorCodeSent));
    }

    public async Task<Result<TwoFactorAuthResponseDto>> ValidateTwoFactorCodeAsync(string userId, string code, CancellationToken cancellationToken = default)
    {
        var validCode = await twoFactorAuthRepository.GetValidCodeAsync(userId, code, cancellationToken);
        if (validCode is null)
        {
            return Result<TwoFactorAuthResponseDto>.Failure(DomainConstants.Errors.InvalidCode);
        }

        await twoFactorAuthRepository.MarkCodeAsUsedAsync(userId, code, cancellationToken);
        return Result<TwoFactorAuthResponseDto>.Success(new TwoFactorAuthResponseDto(true));
    }

    public async Task<Result<AuthResponseDto>> LoginWithTwoFactorAsync(string userId, string code, CancellationToken cancellationToken = default)
    {
        var validateResult = await ValidateTwoFactorCodeAsync(userId, code, cancellationToken);
        if (!validateResult.IsSuccess)
        {
            return Result<AuthResponseDto>.Failure(validateResult.ErrorMessages);
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result<AuthResponseDto>.Failure(DomainConstants.Errors.UserNotFound);
        }

        await signInManager.SignInAsync(user, isPersistent: true);
        user.LastLoginAt = DateTime.UtcNow;
        await userManager.UpdateAsync(user);

        return Result<AuthResponseDto>.Success(new AuthResponseDto(true, DomainConstants.Messages.LoginSuccess, UserId: user.Id));
    }

    public async Task<Result> ForgotPasswordAsync(ForgotPasswordRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            // セキュリティ: ユーザーが存在しなくても成功を返す
            return Result.Success();
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var baseUrl = configuration["AppUrl"];
        var resetLink = $"{baseUrl}/reset-password?email={request.Email}&token={encodedToken}";

        await emailRepository.SendPasswordResetLinkAsync(request.Email, resetLink, cancellationToken);
        logger.LogInformation("パスワードリセットメール送信: {Email}", request.Email);

        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(ResetPasswordRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result.Failure(DomainConstants.Errors.UserNotFound);
        }

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
        var result = await userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);

        if (!result.Succeeded)
        {
            return Result.Failure(result.Errors.Select(e => e.Description));
        }

        user.LastPasswordChangedAt = DateTime.UtcNow;
        user.RequirePasswordChange = false;
        await userManager.UpdateAsync(user);

        logger.LogInformation("パスワードリセット成功: {Email}", request.Email);
        return Result.Success();
    }

    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(DomainConstants.Errors.UserNotFound);
        }

        var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            return Result.Failure(result.Errors.Select(e => e.Description));
        }

        user.LastPasswordChangedAt = DateTime.UtcNow;
        user.RequirePasswordChange = false;
        await userManager.UpdateAsync(user);

        logger.LogInformation("パスワード変更成功: {UserId}", userId);
        return Result.Success();
    }

    public async Task<Result> ConfirmEmailAsync(string userId, string token, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(DomainConstants.Errors.UserNotFound);
        }

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        var result = await userManager.ConfirmEmailAsync(user, decodedToken);

        if (!result.Succeeded)
        {
            return Result.Failure(result.Errors.Select(e => e.Description));
        }

        logger.LogInformation("メール確認成功: {Email}", user.Email);
        return Result.Success();
    }

    public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result.Success(); // セキュリティ
        }

        if (await userManager.IsEmailConfirmedAsync(user))
        {
            return Result.Failure("このメールアドレスは既に確認済みです");
        }

        var confirmationLink = await GenerateEmailConfirmationLinkAsync(user);
        await emailRepository.SendEmailConfirmationAsync(user.Email!, confirmationLink, cancellationToken);

        return Result.Success();
    }

    public async Task<Result<UserInfoResponseDto>> GetUserInfoAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result<UserInfoResponseDto>.Failure(DomainConstants.Errors.UserNotFound);
        }

        return Result<UserInfoResponseDto>.Success(new UserInfoResponseDto(
            user.Id,
            user.Email!,
            user.FirstName,
            user.LastName,
            user.OrganizationName,
            user.CreatedAt,
            user.LastLoginAt
        ));
    }

    public async Task<Result<UserProfileDto>> GetUserProfileAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result<UserProfileDto>.Failure(DomainConstants.Errors.UserNotFound);
        }

        var history = await installHistoryRepository.GetByUserIdAsync(userId, cancellationToken);
        var historyDtos = history.Select(h => new InstallHistoryDto(h.Id, h.InstalledAt, h.OS)).ToList();

        return Result<UserProfileDto>.Success(new UserProfileDto(
            user.Id,
            user.Email!,
            user.FirstName,
            user.LastName,
            user.OrganizationName,
            historyDtos
        ));
    }

    public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(DomainConstants.Errors.UserNotFound);
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.OrganizationName = request.OrganizationName ?? string.Empty;

        await userManager.UpdateAsync(user);
        return Result.Success();
    }

    private async Task<string> GenerateEmailConfirmationLinkAsync(ApplicationUser user)
    {
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var baseUrl = configuration["AppUrl"];
        return $"{baseUrl}/confirm-email?userId={user.Id}&code={encodedToken}";
    }
}
