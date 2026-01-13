using Domain.Abstractions;
using Domain.Dtos;

namespace Domain.ServiceInterfaces;

/// <summary>
/// 認証サービスインターフェース
/// </summary>
public interface IAuthenticationService
{
    // 登録
    Task<Result<AuthResponseDto>> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponseDto>> RegisterAdminAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);

    // ログイン/ログアウト
    Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> LogoutAsync(CancellationToken cancellationToken = default);

    // 2要素認証
    Task<Result<TwoFactorAuthResponseDto>> GenerateAndSendTwoFactorCodeAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result<TwoFactorAuthResponseDto>> ValidateTwoFactorCodeAsync(string userId, string code, CancellationToken cancellationToken = default);
    Task<Result<AuthResponseDto>> LoginWithTwoFactorAsync(string userId, string code, CancellationToken cancellationToken = default);

    // パスワード
    Task<Result> ForgotPasswordAsync(ForgotPasswordRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> ResetPasswordAsync(ResetPasswordRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequestDto request, CancellationToken cancellationToken = default);

    // メール確認
    Task<Result> ConfirmEmailAsync(string userId, string token, CancellationToken cancellationToken = default);
    Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequestDto request, CancellationToken cancellationToken = default);

    // ユーザー情報
    Task<Result<UserInfoResponseDto>> GetUserInfoAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result<UserProfileDto>> GetUserProfileAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequestDto request, CancellationToken cancellationToken = default);
}
