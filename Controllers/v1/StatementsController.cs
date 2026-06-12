using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi_money_management.DTOs;
using WebApi_money_management.Services;

namespace WebApi_money_management.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/statements")]
public class StatementsController(IStatementService statementService) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<ActionResult<UploadStatementResponse>> Upload(
        [FromForm] string bank,
        IFormFile file,
        [FromForm] string? startDate,
        [FromForm] string? endDate)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await statementService.UploadAsync(userId, bank, file, startDate, endDate);
        return Created($"api/v1/statements/{response.StatementId}", response);
    }
}
