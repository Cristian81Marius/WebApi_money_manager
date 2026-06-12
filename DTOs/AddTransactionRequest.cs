using System.ComponentModel.DataAnnotations;

namespace WebApi_money_management.DTOs;

public class AddTransactionRequest
{
    [Required]
    [RegularExpression(@"^(income|expense)$", ErrorMessage = "type must be 'income' or 'expense'.")]
    public string Type { get; set; } = string.Empty;

    [Required]
    [StringLength(255, MinimumLength = 1)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "amount must be greater than 0.")]
    public decimal Amount { get; set; }

    [Required]
    public string Date { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Category { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Notes { get; set; }
}
