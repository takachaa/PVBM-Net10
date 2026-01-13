using Microsoft.Extensions.Logging;
using Domain.Abstractions;
using Domain.Dtos;
using Domain.Repositories;
using Domain.ServiceInterfaces;

namespace Domain.Services;

/// <summary>
/// 問い合わせサービス（C# 14: Primary Constructor）
/// </summary>
public class ContactService(
    IEmailRepository emailRepository,
    ILogger<ContactService> logger) : IContactService
{
    public async Task<Result> SendContactInquiryAsync(ContactRequestDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            await emailRepository.SendContactInquiryAsync(
                request.Name,
                request.Email,
                request.Subject,
                request.Message,
                cancellationToken);

            logger.LogInformation("問い合わせ送信成功: {Email}", request.Email);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "問い合わせ送信失敗: {Email}", request.Email);
            return Result.Failure("問い合わせの送信に失敗しました");
        }
    }
}
