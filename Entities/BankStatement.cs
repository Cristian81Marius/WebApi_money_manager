namespace WebApi_money_management.Entities;

public class BankStatement
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string Bank { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public int ImportedCount { get; set; }
    public DateTime UploadedAt { get; set; }
}
