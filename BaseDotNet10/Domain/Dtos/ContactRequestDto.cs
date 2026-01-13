namespace Domain.Dtos;

// 問い合わせ関連DTO

public record ContactRequestDto(
    string Name,
    string Email,
    string Subject,
    string Message
);
