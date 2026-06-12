using System.ComponentModel.DataAnnotations;

namespace WebApi_money_management.DTOs;

public class StatisticsQuery
{
    [RegularExpression(@"^\d{4}-\d{2}$", ErrorMessage = "fromMonth must be in YYYY-MM format.")]
    public string? FromMonth { get; set; }

    [RegularExpression(@"^\d{4}-\d{2}$", ErrorMessage = "toMonth must be in YYYY-MM format.")]
    public string? ToMonth { get; set; }
}
