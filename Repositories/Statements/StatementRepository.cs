using WebApi_money_management.Data;
using WebApi_money_management.Entities;

namespace WebApi_money_management.Repositories.Statements;

public class StatementRepository(AppDbContext db) : IStatementRepository
{
    public async Task<BankStatement> CreateAsync(BankStatement statement)
    {
        statement.Id = Guid.NewGuid();
        statement.UploadedAt = DateTime.UtcNow;
        db.BankStatements.Add(statement);
        await db.SaveChangesAsync();
        return statement;
    }
}
