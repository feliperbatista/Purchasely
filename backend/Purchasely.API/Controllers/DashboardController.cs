using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Purchasely.Application.Features.Dashboard.Queries;

namespace Purchasely.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DashboardController(IMediator mediator) : ControllerBase
{
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetDashboardStatsQuery(), cancellationToken);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return Ok(result.Value);
    }
}