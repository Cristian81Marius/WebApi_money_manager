using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi_money_management.DTOs;
using WebApi_money_management.Services;

namespace WebApi_money_management.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/statistics")]
public class StatisticsController(IStatisticsService statisticsService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<StatisticsResponse>> GetStatistics([FromQuery] StatisticsQuery query)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await statisticsService.GetStatisticsAsync(userId, query.FromMonth, query.ToMonth);
        return Ok(response);
    }
}
