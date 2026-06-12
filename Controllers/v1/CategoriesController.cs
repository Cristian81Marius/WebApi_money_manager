using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi_money_management.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/categories")]
public class CategoriesController : ControllerBase
{
    private static readonly string[] Income =
    [
        "Salary", "Freelance", "Business", "Investment", "Gift", "Refund", "Other"
    ];

    private static readonly string[] Expense =
    [
        "Food & Drink", "Transport", "Utilities", "Health", "Shopping",
        "Entertainment", "Housing", "Education", "Other"
    ];

    [HttpGet]
    public IActionResult Get() => Ok(new { income = Income, expense = Expense });
}
