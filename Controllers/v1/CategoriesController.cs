using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebApi_money_management.Data;

namespace WebApi_money_management.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/categories")]
public class CategoriesController(AppDbContext db, IMemoryCache cache) : ControllerBase
{
    private const string CacheKey = "categories";

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!cache.TryGetValue(CacheKey, out var categories))
        {
            var rows = await db.Categories.ToListAsync();

            categories = new
            {
                income  = rows.Where(c => c.Type == "income").Select(c => c.Name),
                expense = rows.Where(c => c.Type == "expense").Select(c => c.Name)
            };

            cache.Set(CacheKey, categories, TimeSpan.FromDays(1));
        }

        return Ok(categories);
    }
}
