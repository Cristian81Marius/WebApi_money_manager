using WebApi_money_management.Data;
using WebApi_money_management.Entities;

namespace WebApi_money_management.Repositories.Transactions;

public class TransactionRepository(AppDbContext db) : ITransactionRepository
{
    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
        transaction.Id = Guid.NewGuid();
        transaction.CreatedAt = DateTime.UtcNow;
        db.Transactions.Add(transaction);
        await db.SaveChangesAsync();
        return transaction;
    }
}
