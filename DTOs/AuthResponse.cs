namespace WebApi_money_management.DTOs;

public record AuthResponse(
    Guid Id,
    string Name,
    string Email,
    string Token
);
