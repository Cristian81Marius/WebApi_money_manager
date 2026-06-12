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
    public DbSet<Bank> Banks => Set<Bank>();
    public DbSet<Category> Categories => Set<Category>();

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

        modelBuilder.Entity<Bank>(e =>
        {
            e.HasKey(b => b.Id);
            e.Property(b => b.Id).HasMaxLength(20).IsRequired();
            e.Property(b => b.Name).HasMaxLength(100).IsRequired();
            e.HasData(
                new Bank { Id = "BT",         Name = "Banca Transilvania" },
                new Bank { Id = "ING",        Name = "ING Bank" },
                new Bank { Id = "BCR",        Name = "BCR" },
                new Bank { Id = "BRD",        Name = "BRD" },
                new Bank { Id = "Raiffeisen", Name = "Raiffeisen Bank" },
                new Bank { Id = "UniCredit",  Name = "UniCredit Bank" },
                new Bank { Id = "CEC",        Name = "CEC Bank" },
                new Bank { Id = "Other",      Name = "Other" }
            );
        });

        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Type).HasMaxLength(10).IsRequired();
            e.Property(c => c.Name).HasMaxLength(100).IsRequired();
            e.HasData(
                new Category { Id = 1,  Type = "income",  Name = "Salary" },
                new Category { Id = 2,  Type = "income",  Name = "Freelance" },
                new Category { Id = 3,  Type = "income",  Name = "Business" },
                new Category { Id = 4,  Type = "income",  Name = "Investment" },
                new Category { Id = 5,  Type = "income",  Name = "Gift" },
                new Category { Id = 6,  Type = "income",  Name = "Refund" },
                new Category { Id = 7,  Type = "income",  Name = "Other" },
                new Category { Id = 8,  Type = "expense", Name = "Food & Drink" },
                new Category { Id = 9,  Type = "expense", Name = "Transport" },
                new Category { Id = 10, Type = "expense", Name = "Utilities" },
                new Category { Id = 11, Type = "expense", Name = "Health" },
                new Category { Id = 12, Type = "expense", Name = "Shopping" },
                new Category { Id = 13, Type = "expense", Name = "Entertainment" },
                new Category { Id = 14, Type = "expense", Name = "Housing" },
                new Category { Id = 15, Type = "expense", Name = "Education" },
                new Category { Id = 16, Type = "expense", Name = "Other" }
            );
        });
    }
}
