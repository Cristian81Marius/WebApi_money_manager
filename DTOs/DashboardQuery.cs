using System.ComponentModel.DataAnnotations;

namespace WebApi_money_management.DTOs;

public class DashboardQuery
{
    [RegularExpression(@"^\d{4}-\d{2}$", ErrorMessage = "month must be in YYYY-MM format.")]
    public string? Month { get; set; }
}
