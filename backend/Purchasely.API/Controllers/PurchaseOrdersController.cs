using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Purchasely.Application.DTOs;
using Purchasely.Application.Features.PurchaseOrders.Commands;
using Purchasely.Application.Features.PurchaseOrders.Queries;
using Purchasely.Domain.Enums;

namespace Purchasely.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PurchaseOrdersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PurchaseOrderStatus? status,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] Guid? supplierId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default
    )
    {
        var result = await mediator.Send(new GetPurchaseOrdersQuery(page, pageSize, status, from, to, supplierId), cancellationToken);

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPurchaseOrderByIdQuery(id), cancellationToken);

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return Ok(result.Value);
    }

    [Authorize(Roles = "Buyer,Admin")]  
    [HttpPost("{id:guid}/issue")]
    public async Task<IActionResult> Issue([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new IssuePurchaseOrderCommand(id), cancellationToken);

        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, result.Errors);
    }

    [Authorize(Roles = "Buyer,Admin")]
    [Consumes("multipart/form-data")]
    [HttpPost("{id:guid}/receive")]
    public async Task<IActionResult> Receive(
        [FromRoute] Guid id,
        [FromForm] List<PurchaseOrderLinesReceived> lines,
        [FromForm] List<IFormFile> proofs,
        CancellationToken cancellationToken)
    {
        if (proofs is null || proofs.Count == 0)
            return BadRequest("At least one proof of receipt file is required");

        var allowedTypes = new[] { "application/pdf", "image/png", "image/jpeg" };
        var invalidFile = proofs.FirstOrDefault(f => !allowedTypes.Contains(f.ContentType));
        if (invalidFile is not null)
            return BadRequest($"{invalidFile.FileName} is not allowed file type");

        const long maxSize = 5 * 1024 * 1024;
        var overSizedFile = proofs.FirstOrDefault(f => f.Length > maxSize);
        if (invalidFile is not null)
            return BadRequest($"{invalidFile.FileName} exceeds the 5MB size limit");

        var files = proofs.Select(f => new ReceiptFileDto(
            f.FileName,
            f.ContentType,
            f.Length,
            f.OpenReadStream()
        )).ToList();

        var result = await mediator.Send(new ReceivePurchaseOrderCommand(id, lines, files), cancellationToken);

        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, result.Errors);
    }

    [Authorize(Roles = "Buyer,Admin")]
    [HttpPost("{id:guid}/close")]
    public async Task<IActionResult> Close([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ClosePurchaseOrderCommand(id), cancellationToken);

        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, result.Errors);
    }


    [Authorize(Roles = "Buyer,Admin")]
    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel([FromRoute] Guid id, [FromBody] RejectRequisitionRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CancelPurchaseOrderCommand(id, request.Reason), cancellationToken);

        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, result.Errors);
    }
}