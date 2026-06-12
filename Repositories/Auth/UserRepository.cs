using Microsoft.EntityFrameworkCore;
using WebApi_money_management.Data;
using WebApi_money_management.Entities;

namespace WebApi_money_management.Repositories.Auth;

public class UserRepository(AppDbContext db) : IUserRepository
{
    public Task<User?> FindByEmailAsync(string email) =>
        db.Users.FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant());

    public Task<User?> FindByIdAsync(Guid id) =>
        db.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User> CreateAsync(User user)
    {
        user.Id = Guid.NewGuid();
        user.CreatedAt = DateTime.UtcNow;
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    public Task<bool> ExistsByEmailAsync(string email) =>
        db.Users.AnyAsync(u => u.Email == email.ToLowerInvariant());
}
