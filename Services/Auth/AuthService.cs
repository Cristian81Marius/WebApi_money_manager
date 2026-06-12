using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApi_money_management.DTOs;
using WebApi_money_management.Entities;
using WebApi_money_management.Exceptions;
using WebApi_money_management.Repositories;
using WebApi_money_management.Services;

namespace WebApi_money_management.Services.Auth;

public class AuthService(
    IUserRepository userRepository,
    ITokenBlacklistRepository tokenBlacklist,
    IConfiguration configuration) : IAuthService
{
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await userRepository.FindByEmailAsync(request.Email)
            ?? throw new InvalidCredentialsException();

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new InvalidCredentialsException();

        var token = GenerateToken(user);
        return new AuthResponse(user.Id, user.Name, user.Email, token);
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await userRepository.ExistsByEmailAsync(request.Email))
            throw new EmailTakenException();

        var user = new User
        {
            Name = request.Name,
            Email = request.Email.ToLowerInvariant(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        user = await userRepository.CreateAsync(user);
        var token = GenerateToken(user);
        return new AuthResponse(user.Id, user.Name, user.Email, token);
    }

    public async Task LogoutAsync(string token)
    {
        await tokenBlacklist.RevokeAsync(token);
    }

    public async Task<AuthResponse> GetCurrentUserAsync(Guid userId, string token)
    {
        var user = await userRepository.FindByIdAsync(userId)
            ?? throw new UnauthorizedException();

        return new AuthResponse(user.Id, user.Name, user.Email, token);
    }

    private string GenerateToken(User user)
    {
        var jwtSection = configuration.GetSection("Jwt");
        var secret = jwtSection["Secret"]
            ?? throw new InvalidOperationException("Jwt:Secret is not configured.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = int.Parse(jwtSection["ExpirationMinutes"] ?? "1440");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiration),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
