using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi_money_management.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/banks")]
public class BanksController : ControllerBase
{
    private static readonly object[] Banks =
    [
        new { id = "BT",         name = "Banca Transilvania" },
        new { id = "ING",        name = "ING Bank" },
        new { id = "BCR",        name = "BCR" },
        new { id = "BRD",        name = "BRD" },
        new { id = "Raiffeisen", name = "Raiffeisen Bank" },
        new { id = "UniCredit",  name = "UniCredit Bank" },
        new { id = "CEC",        name = "CEC Bank" },
        new { id = "Other",      name = "Other" }
    ];

    [HttpGet]
    public IActionResult Get() => Ok(Banks);
}
