using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi_money_management.DTOs;
using WebApi_money_management.Services;

namespace WebApi_money_management.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/dashboard")]
public class DashboardController(IDashboardService dashboardService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<DashboardResponse>> GetSummary([FromQuery] DashboardQuery query)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await dashboardService.GetSummaryAsync(userId, query.Month);
        return Ok(response);
    }
}
