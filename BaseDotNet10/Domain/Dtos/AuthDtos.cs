namespace Domain.Dtos;

// 認証関連DTOs（C# 14: record構文）

public record RegisterRequestDto(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? OrganizationName = null
);

public record LoginRequestDto(
    string Email,
    string Password
);

public record AuthResponseDto(
    bool Success,
    string? Message = null,
    bool RequiresTwoFactor = false,
    string? UserId = null
);

public record AuthResultDto(
    bool IsSuccess,
    string? Message = null,
    IReadOnlyList<string>? Errors = null
);

public record TwoFactorCodeRequestDto(
    string UserId
);

public record VerifyCodeRequestDto(
    string UserId,
    string Code
);

public record TwoFactorAuthResponseDto(
    bool Success,
    string? Message = null
);

public record TwoFactorAuthInfoDto(
    bool IsEnabled,
    bool HasAuthenticator
);

public record ForgotPasswordRequestDto(
    string Email
);

public record ResetPasswordRequestDto(
    string Email,
    string Token,
    string NewPassword
);

public record ChangePasswordRequestDto(
    string CurrentPassword,
    string NewPassword
);

public record ResendConfirmationEmailRequestDto(
    string Email
);
