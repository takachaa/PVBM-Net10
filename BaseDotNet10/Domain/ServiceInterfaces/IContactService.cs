using Domain.Abstractions;
using Domain.Dtos;

namespace Domain.ServiceInterfaces;

/// <summary>
/// 問い合わせサービスインターフェース
/// </summary>
public interface IContactService
{
    Task<Result> SendContactInquiryAsync(ContactRequestDto request, CancellationToken cancellationToken = default);
}
