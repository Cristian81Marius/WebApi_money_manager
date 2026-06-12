using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_money_management.Data;

namespace WebApi_money_management.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/banks")]
public class BanksController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var banks = await db.Banks
            .OrderBy(b => b.Id == "Other")
            .ThenBy(b => b.Name)
            .Select(b => new { id = b.Id, name = b.Name })
            .ToListAsync();

        return Ok(banks);
    }
}
