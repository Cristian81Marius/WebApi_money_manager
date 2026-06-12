namespace WebApi_money_management.Entities;

public class UserPreference
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string Language { get; set; } = "en";
    public string Theme { get; set; } = "light";
    public string? ProfilePicture { get; set; }
}
