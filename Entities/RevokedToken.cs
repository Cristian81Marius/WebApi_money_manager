namespace WebApi_money_management.Entities;

public class RevokedToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime RevokedAt { get; set; }
}
