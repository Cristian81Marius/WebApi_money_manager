using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebApi_money_management.Data;

namespace WebApi_money_management.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/banks")]
public class BanksController(AppDbContext db, IMemoryCache cache) : ControllerBase
{
    private const string CacheKey = "banks";

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!cache.TryGetValue(CacheKey, out var banks))
        {
            banks = await db.Banks
                .OrderBy(b => b.Id == "Other")
                .ThenBy(b => b.Name)
                .Select(b => new { id = b.Id, name = b.Name })
                .ToListAsync();

            cache.Set(CacheKey, banks, TimeSpan.FromDays(1));
        }

        return Ok(banks);
    }
}
