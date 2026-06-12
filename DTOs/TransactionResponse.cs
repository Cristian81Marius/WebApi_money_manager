namespace WebApi_money_management.DTOs;

public record TransactionResponse(
    Guid TransactionId,
    string Type,
    string Description,
    decimal Amount,
    DateOnly Date,
    string Category,
    string? Notes
);
