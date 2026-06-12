namespace WebApi_money_management.DTOs;

public class UserPreferencesResponse
{
    public string Language { get; set; } = string.Empty;
    public string Theme { get; set; } = string.Empty;
    public string? ProfilePicture { get; set; }
}
