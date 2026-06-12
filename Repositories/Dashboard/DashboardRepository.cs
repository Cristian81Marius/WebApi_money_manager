using Microsoft.EntityFrameworkCore;
using WebApi_money_management.Data;
using WebApi_money_management.Entities;

namespace WebApi_money_management.Repositories.Dashboard;

public class DashboardRepository(AppDbContext db) : IDashboardRepository
{
    public async Task<decimal> GetTotalBalanceAsync(Guid userId)
    {
        var income = await db.Transactions
            .Where(t => t.UserId == userId && t.Type == "income")
            .SumAsync(t => (decimal?)t.Amount) ?? 0;

        var expenses = await db.Transactions
            .Where(t => t.UserId == userId && t.Type == "expense")
            .SumAsync(t => (decimal?)t.Amount) ?? 0;

        return income - expenses;
    }

    public Task<decimal> GetIncomeForMonthAsync(Guid userId, int year, int month)
    {
        var start = new DateOnly(year, month, 1);
        var end = start.AddMonths(1);

        return db.Transactions
            .Where(t => t.UserId == userId && t.Type == "income"
                     && t.Date >= start && t.Date < end)
            .SumAsync(t => (decimal?)t.Amount)
            .ContinueWith(t => t.Result ?? 0);
    }

    public Task<decimal> GetExpensesForMonthAsync(Guid userId, int year, int month)
    {
        var start = new DateOnly(year, month, 1);
        var end = start.AddMonths(1);

        return db.Transactions
            .Where(t => t.UserId == userId && t.Type == "expense"
                     && t.Date >= start && t.Date < end)
            .SumAsync(t => (decimal?)t.Amount)
            .ContinueWith(t => t.Result ?? 0);
    }

    public Task<int> GetTransactionCountAsync(Guid userId) =>
        db.Transactions.CountAsync(t => t.UserId == userId);

    public async Task<IEnumerable<Transaction>> GetRecentTransactionsAsync(Guid userId, int count = 10) =>
        await db.Transactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Date)
            .ThenByDescending(t => t.CreatedAt)
            .Take(count)
            .ToListAsync();
}
