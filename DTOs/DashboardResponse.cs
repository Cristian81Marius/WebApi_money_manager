namespace WebApi_money_management.DTOs;

public record DashboardResponse(
    decimal TotalBalance,
    decimal IncomeThisMonth,
    decimal ExpensesThisMonth,
    int TransactionCount,
    IEnumerable<RecentTransactionDto> RecentTransactions
);
