namespace Domain.Dtos;

// ユーザー関連DTOs

public record UserInfoResponseDto(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    string? OrganizationName,
    DateTime CreatedAt,
    DateTime? LastLoginAt
);

public record UserResponseDto(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    bool EmailConfirmed,
    string? OrganizationName
);

public record UserProfileDto(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    string? OrganizationName,
    IReadOnlyList<InstallHistoryDto> InstallHistory
);

public record InstallHistoryDto(
    int Id,
    DateTime InstalledAt,
    string OS
);

public record UpdateProfileRequestDto(
    string FirstName,
    string LastName,
    string? OrganizationName
);
