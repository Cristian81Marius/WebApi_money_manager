using WebApi_money_management.DTOs;
using WebApi_money_management.Repositories;

namespace WebApi_money_management.Services.Dashboard;

public class DashboardService(IDashboardRepository dashboardRepository) : IDashboardService
{
    public async Task<DashboardResponse> GetSummaryAsync(Guid userId, string? month)
    {
        var (year, monthNum) = ParseMonth(month);

        var totalBalance      = await dashboardRepository.GetTotalBalanceAsync(userId);
        var incomeThisMonth   = await dashboardRepository.GetIncomeForMonthAsync(userId, year, monthNum);
        var expensesThisMonth = await dashboardRepository.GetExpensesForMonthAsync(userId, year, monthNum);
        var transactionCount  = await dashboardRepository.GetTransactionCountAsync(userId);
        var recent            = await dashboardRepository.GetRecentTransactionsAsync(userId);

        var recentDtos = recent.Select(t => new RecentTransactionDto(
            t.Id,
            t.Type,
            t.Description,
            t.Amount,
            t.Date,
            t.Category));

        return new DashboardResponse(
            totalBalance,
            incomeThisMonth,
            expensesThisMonth,
            transactionCount,
            recentDtos);
    }

    private static (int year, int month) ParseMonth(string? month)
    {
        if (month is null)
        {
            var now = DateTime.UtcNow;
            return (now.Year, now.Month);
        }

        var parts = month.Split('-');
        return (int.Parse(parts[0]), int.Parse(parts[1]));
    }
}
