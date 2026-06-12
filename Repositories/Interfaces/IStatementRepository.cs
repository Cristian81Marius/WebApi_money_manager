using WebApi_money_management.Entities;

namespace WebApi_money_management.Repositories;

public interface IStatementRepository
{
    Task<BankStatement> CreateAsync(BankStatement statement);
}
