namespace WebApi_money_management.DTOs;

public record RecentTransactionDto(
    Guid Id,
    string Type,
    string Description,
    decimal Amount,
    DateOnly Date,
    string Category
);
