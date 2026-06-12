using WebApi_money_management.DTOs;

namespace WebApi_money_management.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task LogoutAsync(string token);
    Task<AuthResponse> GetCurrentUserAsync(Guid userId, string token);
}
