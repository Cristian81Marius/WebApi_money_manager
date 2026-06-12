using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi_money_management.DTOs;
using WebApi_money_management.Services;

namespace WebApi_money_management.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/users")]
public class UsersController(IUserPreferenceService preferenceService) : ControllerBase
{
    [HttpGet("preferences")]
    public async Task<ActionResult<UserPreferencesResponse>> GetPreferences()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await preferenceService.GetAsync(userId));
    }

    [HttpPatch("preferences")]
    public async Task<ActionResult<UserPreferencesResponse>> UpdatePreferences([FromBody] UpdatePreferencesRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await preferenceService.UpdateAsync(userId, request));
    }
}
