using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi_money_management.DTOs;
using WebApi_money_management.Services;

namespace WebApi_money_management.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/transactions")]
public class TransactionsController(ITransactionService transactionService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<TransactionResponse>> Add([FromBody] AddTransactionRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await transactionService.AddAsync(userId, request);
        return CreatedAtAction(nameof(Add), new { response.TransactionId }, response);
    }
}
