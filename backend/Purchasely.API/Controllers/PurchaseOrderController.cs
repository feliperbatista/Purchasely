using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Purchasely.Application.DTOs;
using Purchasely.Application.Features.PurchaseOrders.Queries;

namespace Purchasely.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PurchaseOrdersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPurchaseOrdersQuery(), cancellationToken);

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPurchaseOrderByIdQuery(id), cancellationToken);

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/submit")]
    public async Task<IActionResult> Submit([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return NoContent();
    }

    [Authorize(Roles = "Manager,Admin")]
    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> Approve([FromRoute] Guid id, CancellationToken cancellationToken)
    {
         return NoContent();
    }

    [Authorize(Roles = "Manager,Admin")]
    [HttpPost("{id:guid}/remove-approval")]
    public async Task<IActionResult> RemoveApproval([FromRoute] Guid id, CancellationToken cancellationToken)
    {
         return NoContent();
    }


    [Authorize(Roles = "Manager,Admin")]
    [HttpPost("{id:guid}/reject")]
    public async Task<IActionResult> Reject([FromRoute] Guid id, [FromBody] RejectRequisitionRequest request, CancellationToken cancellationToken)
    {
         return NoContent();
    }

    [Authorize(Roles = "Buyer,Admin")]
    [HttpPost("{id:guid}/convert-to-po")]
    public async Task<IActionResult> ConvertToPO([FromRoute] Guid id, CancellationToken cancellationToken)
    {
         return NoContent();
    }
}