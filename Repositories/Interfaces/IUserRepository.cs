using WebApi_money_management.Entities;

namespace WebApi_money_management.Repositories;

public interface IUserRepository
{
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByIdAsync(Guid id);
    Task<User> CreateAsync(User user);
    Task<bool> ExistsByEmailAsync(string email);
}
