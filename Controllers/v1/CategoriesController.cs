using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_money_management.Data;

namespace WebApi_money_management.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/categories")]
public class CategoriesController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var categories = await db.Categories.ToListAsync();

        return Ok(new
        {
            income  = categories.Where(c => c.Type == "income").Select(c => c.Name),
            expense = categories.Where(c => c.Type == "expense").Select(c => c.Name)
        });
    }
}
