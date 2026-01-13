using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using Domain.Repositories;

namespace Infrastructure.Repositories;

/// <summary>
/// SendGridメールリポジトリ実装
/// </summary>
public class SendGridEmailRepository(
    IConfiguration configuration,
    ILogger<SendGridEmailRepository> logger) : IEmailRepository
{
    private readonly string _apiKey = configuration["SendGrid:ApiKey"] ?? throw new InvalidOperationException("SendGrid:ApiKey is not configured");
    private readonly string _fromEmail = configuration["SendGrid:FromEmail"] ?? "noreply@example.com";
    private readonly string _adminEmail = configuration["SendGrid:AdminEmail"] ?? "";

    public async Task<bool> SendAuthenticationCodeAsync(string email, string code, CancellationToken cancellationToken = default)
    {
        var subject = "認証コード";
        var content = $"あなたの認証コードは {code} です。このコードは10分間有効です。";
        return await SendEmailAsync(email, subject, content);
    }

    public async Task<bool> SendPasswordResetLinkAsync(string email, string resetLink, CancellationToken cancellationToken = default)
    {
        var subject = "パスワードリセット";
        var content = $"パスワードをリセットするには、次のリンクをクリックしてください: {resetLink}";
        return await SendEmailAsync(email, subject, content);
    }

    public async Task<bool> SendEmailConfirmationAsync(string email, string confirmationLink, CancellationToken cancellationToken = default)
    {
        var subject = "メールアドレスの確認";
        var content = $"メールアドレスを確認するには、次のリンクをクリックしてください: {confirmationLink}";
        return await SendEmailAsync(email, subject, content);
    }

    public async Task<bool> SendContactInquiryAsync(string name, string email, string subject, string message, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(_adminEmail))
        {
            logger.LogWarning("AdminEmail is not configured");
            return false;
        }

        var emailSubject = $"問い合わせ: {subject}";
        var content = $"""
            名前: {name}
            メール: {email}

            メッセージ:
            {message}
            """;

        return await SendEmailAsync(_adminEmail, emailSubject, content);
    }

    private async Task<bool> SendEmailAsync(string toEmail, string subject, string content)
    {
        try
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);

            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("メール送信成功: {To}", toEmail);
                return true;
            }

            logger.LogWarning("メール送信失敗: {To}, StatusCode: {StatusCode}", toEmail, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "メール送信エラー: {To}", toEmail);
            return false;
        }
    }
}
