namespace Domain.Entities;

/// <summary>
/// 2要素認証エンティティ
/// </summary>
public class TwoFactorAuth
{
    public required string UserId { get; set; }
    public required string Code { get; set; }
    public DateTime ExpirationTime { get; set; }
    public bool IsUsed { get; set; }

    public virtual ApplicationUser User { get; set; } = null!;
}
