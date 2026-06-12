using WebApi_money_management.DTOs;
using WebApi_money_management.Exceptions;
using WebApi_money_management.Repositories;

namespace WebApi_money_management.Services.Statistics;

public class StatisticsService(IStatisticsRepository statisticsRepository) : IStatisticsService
{
    public async Task<StatisticsResponse> GetStatisticsAsync(Guid userId, string? fromMonth, string? toMonth)
    {
        var (toYear, toMonthNum) = ParseYearMonth(toMonth, DateTime.UtcNow);
        var defaultFrom = new DateTime(toYear, toMonthNum, 1).AddMonths(-5);
        var (fromYear, fromMonthNum) = ParseYearMonth(fromMonth, defaultFrom);

        if (new DateOnly(fromYear, fromMonthNum, 1) > new DateOnly(toYear, toMonthNum, 1))
            throw new ValidationException("fromMonth must not be after toMonth.");

        var start = new DateOnly(fromYear, fromMonthNum, 1);
        var end = new DateOnly(toYear, toMonthNum, 1).AddMonths(1);

        var monthlyRaw     = await statisticsRepository.GetMonthlyTotalsAsync(userId, start, end);
        var byCategory     = await statisticsRepository.GetExpensesByCategoryAsync(userId, start, end);
        var totalIncome    = await statisticsRepository.GetTotalByTypeAsync(userId, "income", start, end);
        var totalExpenses  = await statisticsRepository.GetTotalByTypeAsync(userId, "expense", start, end);

        var incomeByMonth   = monthlyRaw.Where(x => x.Type == "income")
                                        .ToDictionary(x => (x.Year, x.Month), x => x.Total);
        var expenseByMonth  = monthlyRaw.Where(x => x.Type == "expense")
                                        .ToDictionary(x => (x.Year, x.Month), x => x.Total);

        var monthlyTrend = GenerateMonthRange(fromYear, fromMonthNum, toYear, toMonthNum)
            .Select(m => new MonthlyPointDto(
                m.Month - 1,
                incomeByMonth.GetValueOrDefault((m.Year, m.Month), 0),
                expenseByMonth.GetValueOrDefault((m.Year, m.Month), 0)));

        var netSavings  = totalIncome - totalExpenses;
        var savingsRate = totalIncome == 0 ? 0 : (int)Math.Round(netSavings / totalIncome * 100);

        return new StatisticsResponse(
            monthlyTrend,
            byCategory.Select(x => new CategoryAmountDto(x.Category, x.Total)),
            totalIncome,
            totalExpenses,
            netSavings,
            Math.Clamp(savingsRate, 0, 100));
    }

    private static (int year, int month) ParseYearMonth(string? value, DateTime fallback)
    {
        if (value is null) return (fallback.Year, fallback.Month);
        var parts = value.Split('-');
        return (int.Parse(parts[0]), int.Parse(parts[1]));
    }

    private static IEnumerable<(int Year, int Month)> GenerateMonthRange(int fromYear, int fromMonth, int toYear, int toMonth)
    {
        var y = fromYear;
        var m = fromMonth;
        while (y < toYear || (y == toYear && m <= toMonth))
        {
            yield return (y, m);
            if (++m > 12) { m = 1; y++; }
        }
    }
}
