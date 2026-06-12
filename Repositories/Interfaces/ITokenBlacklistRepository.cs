namespace WebApi_money_management.Repositories;

public interface ITokenBlacklistRepository
{
    Task RevokeAsync(string token);
    Task<bool> IsRevokedAsync(string token);
}
