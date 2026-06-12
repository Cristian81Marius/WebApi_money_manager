namespace WebApi_money_management.DTOs;

public record StatisticsResponse(
    IEnumerable<MonthlyPointDto> MonthlyTrend,
    IEnumerable<CategoryAmountDto> ExpensesByCategory,
    decimal TotalIncome,
    decimal TotalExpenses,
    decimal NetSavings,
    int SavingsRate
);
