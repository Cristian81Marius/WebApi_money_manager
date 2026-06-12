using Microsoft.EntityFrameworkCore;
using WebApi_money_management.Data;
using WebApi_money_management.Entities;

namespace WebApi_money_management.Repositories.Auth;

public class TokenBlacklistRepository(AppDbContext db) : ITokenBlacklistRepository
{
    public async Task RevokeAsync(string token)
    {
        db.RevokedTokens.Add(new RevokedToken
        {
            Token = token,
            RevokedAt = DateTime.UtcNow
        });
        await db.SaveChangesAsync();
    }

    public Task<bool> IsRevokedAsync(string token) =>
        db.RevokedTokens.AnyAsync(t => t.Token == token);
}
