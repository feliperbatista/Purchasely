using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Purchasely.Application.DTOs;
using Purchasely.Application.Features.Suppliers.Commands;
using Purchasely.Application.Features.Suppliers.Queries;

namespace Purchasely.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SuppliersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetSuppliersQuery(), cancellationToken);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetSupplierByIdQuery(id), cancellationToken);

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSupplierCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSupplierRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateSupplierCommand(id, request.Name, request.Email, request.Phone, request.TaxNumber, request.Address);

        var result = await mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteSupplierCommand(id), cancellationToken);

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return NoContent();
    }

    [HttpPost("{id}/products")]
    public async Task<IActionResult> CreateProduct([FromRoute] Guid id, [FromBody] CreateSupplierProductRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateSupplierProductCommand(id, request.ProductId, request.UnitPrice);

        var result = await mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return Created("", result.Value);
    }

    [HttpDelete("{supplierId}/products/{productId}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid supplierId, [FromRoute] Guid productId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteSupplierProductCommand(supplierId, productId), cancellationToken);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return NoContent();
    }
}