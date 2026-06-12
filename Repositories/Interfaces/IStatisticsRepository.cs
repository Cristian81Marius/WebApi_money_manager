namespace WebApi_money_management.Repositories;

public record MonthlyAggregate(int Year, int Month, string Type, decimal Total);
public record CategoryAggregate(string Category, decimal Total);

public interface IStatisticsRepository
{
    Task<List<MonthlyAggregate>> GetMonthlyTotalsAsync(Guid userId, DateOnly start, DateOnly end);
    Task<List<CategoryAggregate>> GetExpensesByCategoryAsync(Guid userId, DateOnly start, DateOnly end);
    Task<decimal> GetTotalByTypeAsync(Guid userId, string type, DateOnly start, DateOnly end);
}
