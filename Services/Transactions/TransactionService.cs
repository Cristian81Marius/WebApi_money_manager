using WebApi_money_management.DTOs;
using WebApi_money_management.Entities;
using WebApi_money_management.Exceptions;
using WebApi_money_management.Repositories;

namespace WebApi_money_management.Services.Transactions;

public class TransactionService(ITransactionRepository transactionRepository) : ITransactionService
{
    public async Task<TransactionResponse> AddAsync(Guid userId, AddTransactionRequest request)
    {
        if (!DateOnly.TryParseExact(request.Date, "yyyy-MM-dd", out var date))
            throw new ValidationException("date must be a valid date in YYYY-MM-DD format.");

        if (date > DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))
            throw new ValidationException("date must not be more than 1 day in the future.");

        var transaction = new Transaction
        {
            UserId      = userId,
            Type        = request.Type,
            Description = request.Description,
            Amount      = request.Amount,
            Date        = date,
            Category    = request.Category,
            Notes       = request.Notes
        };

        transaction = await transactionRepository.CreateAsync(transaction);

        return new TransactionResponse(
            transaction.Id,
            transaction.Type,
            transaction.Description,
            transaction.Amount,
            transaction.Date,
            transaction.Category,
            transaction.Notes);
    }
}
