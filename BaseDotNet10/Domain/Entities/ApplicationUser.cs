using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

/// <summary>
/// アプリケーションユーザーエンティティ
/// </summary>
public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public DateTime? LastPasswordChangedAt { get; set; }
    public bool RequirePasswordChange { get; set; }
    public string OrganizationName { get; set; } = string.Empty;
}
