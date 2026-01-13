namespace Domain.Entities;

/// <summary>
/// インストール履歴エンティティ
/// </summary>
public class InstallHistory
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public DateTime InstalledAt { get; set; } = DateTime.UtcNow;
    public required string OS { get; set; }

    public ApplicationUser User { get; set; } = null!;
}
