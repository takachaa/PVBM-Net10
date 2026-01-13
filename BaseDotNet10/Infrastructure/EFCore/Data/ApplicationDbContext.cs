using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.EFCore.Data;

/// <summary>
/// アプリケーションデータベースコンテキスト
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<TwoFactorAuth> TwoFactorAuths => Set<TwoFactorAuth>();
    public DbSet<InstallHistory> InstallHistories => Set<InstallHistory>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ApplicationUser
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.OrganizationName).HasMaxLength(200);
        });

        // TwoFactorAuth
        builder.Entity<TwoFactorAuth>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.Code });
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // InstallHistory
        builder.Entity<InstallHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.OS).HasMaxLength(50);
        });
    }
}
