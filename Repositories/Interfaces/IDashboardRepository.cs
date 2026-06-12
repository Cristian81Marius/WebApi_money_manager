using WebApi_money_management.Entities;

namespace WebApi_money_management.Repositories;

public interface IDashboardRepository
{
    Task<decimal> GetTotalBalanceAsync(Guid userId);
    Task<decimal> GetIncomeForMonthAsync(Guid userId, int year, int month);
    Task<decimal> GetExpensesForMonthAsync(Guid userId, int year, int month);
    Task<int> GetTransactionCountAsync(Guid userId);
    Task<IEnumerable<Transaction>> GetRecentTransactionsAsync(Guid userId, int count = 10);
}
