using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Purchasely.Application.DTOs;
using Purchasely.Application.Features.Requisitions.Commands;
using Purchasely.Application.Features.Requisitions.Queries;

namespace Purchasely.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class RequisitionController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRequisitionsQuery(), cancellationToken);

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRequisitionByIdQuery(id), cancellationToken);

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRequisitionCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRequisitionRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateRequisitionCommand(
            id,
            request.Priority,
            request.Justification,
            [.. request.Lines.Select(l => new CreateRequisitionLinesCommand(
                l.ProductId,
                l.QuantityRequested,
                l.EstimatedUnitPrice
        ))]);

        var result = await mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return NoContent();
    }

    [HttpPost("{id:guid}/submit")]
    public async Task<IActionResult> Submit([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new SubmitRequisitionCommand(id), cancellationToken);

        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, result.Errors);
    }

    [Authorize(Roles = "Manager,Admin")]
    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> Approve([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ApproveRequisitionCommand(id), cancellationToken);

        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, result.Errors);
    }

    [Authorize(Roles = "Manager,Admin")]
    [HttpPost("{id:guid}/remove-approval")]
    public async Task<IActionResult> RemoveApproval([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RemoveApprovalRequisitionCommand(id), cancellationToken);

        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, result.Errors);
    }


    [Authorize(Roles = "Manager,Admin")]
    [HttpPost("{id:guid}/reject")]
    public async Task<IActionResult> Reject([FromRoute] Guid id, [FromBody] RejectRequisitionRequest request, CancellationToken cancellationToken)
    {
        var command = new RejectRequisitionCommand(id, request.Reason);

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, result.Errors);
    }

    [Authorize(Roles = "Buyer,Admin")]
    [HttpPost("{id:guid}/convert-to-po")]
    public async Task<IActionResult> ConvertToPO([FromRoute] Guid id, [FromBody] List<CreatePOLineCommand> lineCommands, CancellationToken cancellationToken)
    {
        var command = new ConvertRequisitionToPOCommand(id, lineCommands);

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess 
            ? Created("", result.Value)
            : StatusCode(result.StatusCode, result.Errors);
    }
}