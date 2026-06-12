using Microsoft.EntityFrameworkCore;
using WebApi_money_management.Data;

namespace WebApi_money_management.Repositories.Statistics;

public class StatisticsRepository(AppDbContext db) : IStatisticsRepository
{
    public async Task<List<MonthlyAggregate>> GetMonthlyTotalsAsync(Guid userId, DateOnly start, DateOnly end)
    {
        var rows = await db.Transactions
            .Where(t => t.UserId == userId && t.Date >= start && t.Date < end)
            .GroupBy(t => new { t.Date.Year, t.Date.Month, t.Type })
            .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Type, Total = g.Sum(t => t.Amount) })
            .ToListAsync();

        return [..rows.Select(x => new MonthlyAggregate(x.Year, x.Month, x.Type, x.Total))];
    }

    public async Task<List<CategoryAggregate>> GetExpensesByCategoryAsync(Guid userId, DateOnly start, DateOnly end)
    {
        var rows = await db.Transactions
            .Where(t => t.UserId == userId && t.Type == "expense" && t.Date >= start && t.Date < end)
            .GroupBy(t => t.Category)
            .Select(g => new { Category = g.Key, Total = g.Sum(t => t.Amount) })
            .OrderByDescending(x => x.Total)
            .ToListAsync();

        return [..rows.Select(x => new CategoryAggregate(x.Category, x.Total))];
    }

    public async Task<decimal> GetTotalByTypeAsync(Guid userId, string type, DateOnly start, DateOnly end) =>
        await db.Transactions
            .Where(t => t.UserId == userId && t.Type == type && t.Date >= start && t.Date < end)
            .SumAsync(t => (decimal?)t.Amount) ?? 0;
}
