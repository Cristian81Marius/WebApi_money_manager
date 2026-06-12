using WebApi_money_management.DTOs;

namespace WebApi_money_management.Services;

public interface ITransactionService
{
    Task<TransactionResponse> AddAsync(Guid userId, AddTransactionRequest request);
}
