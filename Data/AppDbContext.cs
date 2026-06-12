using Microsoft.EntityFrameworkCore;
using WebApi_money_management.Entities;

namespace WebApi_money_management.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RevokedToken> RevokedTokens => Set<RevokedToken>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<BankStatement> BankStatements => Set<BankStatement>();
    public DbSet<UserPreference> UserPreferences => Set<UserPreference>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Email).HasMaxLength(255).IsRequired();
            e.Property(u => u.Name).HasMaxLength(100).IsRequired();
            e.Property(u => u.PasswordHash).IsRequired();
        });

        modelBuilder.Entity<RevokedToken>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasIndex(t => t.Token);
            e.Property(t => t.Token).IsRequired();
        });

        modelBuilder.Entity<Transaction>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            e.Property(t => t.Type).HasMaxLength(10).IsRequired();
            e.Property(t => t.Description).HasMaxLength(500).IsRequired();
            e.Property(t => t.Amount).HasPrecision(18, 2).IsRequired();
            e.Property(t => t.Category).HasMaxLength(100).IsRequired();
            e.Property(t => t.Notes).HasMaxLength(1000);
            e.HasIndex(t => new { t.UserId, t.Date });
        });

        modelBuilder.Entity<BankStatement>(e =>
        {
            e.HasKey(s => s.Id);
            e.HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            e.Property(s => s.Bank).HasMaxLength(50).IsRequired();
            e.Property(s => s.FileName).HasMaxLength(255).IsRequired();
        });

        modelBuilder.Entity<UserPreference>(e =>
        {
            e.HasKey(p => p.UserId);
            e.HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<UserPreference>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            e.Property(p => p.Language).HasMaxLength(10).HasDefaultValue("en").IsRequired();
            e.Property(p => p.Theme).HasMaxLength(20).HasDefaultValue("light").IsRequired();
        });
    }
}
