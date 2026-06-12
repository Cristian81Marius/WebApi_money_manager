using WebApi_money_management.Entities;

namespace WebApi_money_management.Repositories;

public interface ITransactionRepository
{
    Task<Transaction> CreateAsync(Transaction transaction);
}
