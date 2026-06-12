using Microsoft.EntityFrameworkCore;
using WebApi_money_management.Entities;

namespace WebApi_money_management.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Transactions.AnyAsync()) return;

        // Use existing user or create the test user if none exists
        var user = await db.Users.FirstOrDefaultAsync();

        if (user is null)
        {
            user = new User
            {
                Id           = Guid.NewGuid(),
                Name         = "John Doe",
                Email        = "test@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test@123"),
                CreatedAt    = DateTime.UtcNow
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        db.Transactions.AddRange(BuildTransactions(user.Id));

        if (!await db.BankStatements.AnyAsync())
            db.BankStatements.AddRange(BuildStatements(user.Id));

        await db.SaveChangesAsync();
    }

    // ── Transactions ──────────────────────────────────────────────────────────

    private static List<Transaction> BuildTransactions(Guid userId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var list  = new List<Transaction>();

        void Add(int monthsAgo, int day, string type, string description,
                 decimal amount, string category, string? notes = null)
        {
            var target = today.AddMonths(-monthsAgo);
            var date   = new DateOnly(target.Year, target.Month,
                                      Math.Min(day, DateTime.DaysInMonth(target.Year, target.Month)));
            list.Add(new Transaction
            {
                Id          = Guid.NewGuid(),
                UserId      = userId,
                Type        = type,
                Description = description,
                Amount      = amount,
                Date        = date,
                Category    = category,
                Notes       = notes,
                CreatedAt   = DateTime.UtcNow
            });
        }

        // ── Month 0 (current) ─────────────────────────────────────────────────
        Add(0,  1, "income",  "Salary",               4500.00m, "Salary");
        Add(0,  3, "expense", "Rent",                  900.00m, "Housing");
        Add(0,  5, "expense", "Lidl",                  156.40m, "Food & Drink",  "Weekly groceries");
        Add(0,  7, "expense", "Netflix",                17.99m, "Entertainment");
        Add(0,  8, "expense", "Bus pass",               45.00m, "Transport");
        Add(0, 10, "expense", "Kaufland",               98.70m, "Food & Drink");
        Add(0, 11, "expense", "Electricity bill",      112.50m, "Utilities");

        // ── Month 1 ───────────────────────────────────────────────────────────
        Add(1,  1, "income",  "Salary",               4500.00m, "Salary");
        Add(1,  5, "income",  "Freelance project",    1200.00m, "Freelance",     "Logo design");
        Add(1,  3, "expense", "Rent",                  900.00m, "Housing");
        Add(1,  6, "expense", "Lidl",                  143.20m, "Food & Drink");
        Add(1,  9, "expense", "Spotify",                10.99m, "Entertainment");
        Add(1, 10, "expense", "Fuel",                   85.00m, "Transport");
        Add(1, 12, "expense", "Doctor visit",           75.00m, "Healthcare");
        Add(1, 14, "expense", "Carrefour",             210.50m, "Food & Drink",  "Bi-weekly shop");
        Add(1, 18, "expense", "Water bill",             38.00m, "Utilities");
        Add(1, 20, "expense", "Gym membership",         40.00m, "Health & Sport");
        Add(1, 22, "expense", "Restaurant",             65.30m, "Food & Drink",  "Birthday dinner");
        Add(1, 25, "expense", "Online course",          29.99m, "Education");

        // ── Month 2 ───────────────────────────────────────────────────────────
        Add(2,  1, "income",  "Salary",               4500.00m, "Salary");
        Add(2,  3, "expense", "Rent",                  900.00m, "Housing");
        Add(2,  7, "expense", "Lidl",                  167.80m, "Food & Drink");
        Add(2,  8, "expense", "Netflix",                17.99m, "Entertainment");
        Add(2, 10, "expense", "Bus pass",               45.00m, "Transport");
        Add(2, 14, "expense", "Pharmacy",               32.50m, "Healthcare");
        Add(2, 15, "expense", "Kaufland",              189.40m, "Food & Drink");
        Add(2, 17, "expense", "Electricity bill",      124.00m, "Utilities");
        Add(2, 20, "expense", "Gym membership",         40.00m, "Health & Sport");
        Add(2, 23, "expense", "New shoes",             129.00m, "Shopping");
        Add(2, 26, "expense", "Taxi",                   22.00m, "Transport");

        // ── Month 3 ───────────────────────────────────────────────────────────
        Add(3,  1, "income",  "Salary",               4500.00m, "Salary");
        Add(3,  2, "income",  "Freelance project",     800.00m, "Freelance",     "Website fixes");
        Add(3,  3, "expense", "Rent",                  900.00m, "Housing");
        Add(3,  5, "expense", "Lidl",                  155.60m, "Food & Drink");
        Add(3,  9, "expense", "Spotify",                10.99m, "Entertainment");
        Add(3, 11, "expense", "Fuel",                   90.00m, "Transport");
        Add(3, 13, "expense", "Kaufland",              201.30m, "Food & Drink");
        Add(3, 16, "expense", "Water bill",             40.00m, "Utilities");
        Add(3, 18, "expense", "Gym membership",         40.00m, "Health & Sport");
        Add(3, 20, "expense", "Cinema",                 28.00m, "Entertainment");
        Add(3, 22, "expense", "Internet bill",          55.00m, "Utilities");
        Add(3, 25, "expense", "Clothing",               89.99m, "Shopping");

        // ── Month 4 ───────────────────────────────────────────────────────────
        Add(4,  1, "income",  "Salary",               4500.00m, "Salary");
        Add(4,  3, "expense", "Rent",                  900.00m, "Housing");
        Add(4,  6, "expense", "Lidl",                  138.90m, "Food & Drink");
        Add(4,  8, "expense", "Netflix",                17.99m, "Entertainment");
        Add(4, 10, "expense", "Bus pass",               45.00m, "Transport");
        Add(4, 11, "expense", "Carrefour",             177.20m, "Food & Drink");
        Add(4, 14, "expense", "Dentist",               150.00m, "Healthcare");
        Add(4, 17, "expense", "Electricity bill",      135.00m, "Utilities");
        Add(4, 19, "expense", "Gym membership",         40.00m, "Health & Sport");
        Add(4, 21, "expense", "Book",                   24.99m, "Education");
        Add(4, 24, "expense", "Restaurant",             48.50m, "Food & Drink");

        // ── Month 5 ───────────────────────────────────────────────────────────
        Add(5,  1, "income",  "Salary",               4500.00m, "Salary");
        Add(5,  5, "income",  "Freelance project",     650.00m, "Freelance",     "SEO audit");
        Add(5,  3, "expense", "Rent",                  900.00m, "Housing");
        Add(5,  7, "expense", "Lidl",                  162.10m, "Food & Drink");
        Add(5,  9, "expense", "Spotify",                10.99m, "Entertainment");
        Add(5, 10, "expense", "Fuel",                   78.00m, "Transport");
        Add(5, 12, "expense", "Kaufland",              193.70m, "Food & Drink");
        Add(5, 15, "expense", "Water bill",             37.50m, "Utilities");
        Add(5, 17, "expense", "Gym membership",         40.00m, "Health & Sport");
        Add(5, 20, "expense", "Pharmacy",               21.00m, "Healthcare");
        Add(5, 22, "expense", "Internet bill",          55.00m, "Utilities");
        Add(5, 25, "expense", "New headphones",        199.00m, "Shopping");

        return list;
    }

    // ── Bank statements ───────────────────────────────────────────────────────

    private static List<BankStatement> BuildStatements(Guid userId)
    {
        var now = DateTime.UtcNow;
        return
        [
            new BankStatement
            {
                Id            = Guid.NewGuid(),
                UserId        = userId,
                Bank          = "BT",
                FileName      = "BT_statement_Q1_2026.csv",
                ImportedCount = 28,
                UploadedAt    = now.AddDays(-45)
            },
            new BankStatement
            {
                Id            = Guid.NewGuid(),
                UserId        = userId,
                Bank          = "ING",
                FileName      = "ING_statement_May2026.pdf",
                ImportedCount = 12,
                UploadedAt    = now.AddDays(-12)
            }
        ];
    }
}
