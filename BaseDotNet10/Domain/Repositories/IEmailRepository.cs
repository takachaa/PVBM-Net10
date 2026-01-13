namespace Domain.Repositories;

/// <summary>
/// メール送信リポジトリインターフェース
/// </summary>
public interface IEmailRepository
{
    Task<bool> SendAuthenticationCodeAsync(string email, string code, CancellationToken cancellationToken = default);
    Task<bool> SendPasswordResetLinkAsync(string email, string resetLink, CancellationToken cancellationToken = default);
    Task<bool> SendEmailConfirmationAsync(string email, string confirmationLink, CancellationToken cancellationToken = default);
    Task<bool> SendContactInquiryAsync(string name, string email, string subject, string message, CancellationToken cancellationToken = default);
}
