using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Purchasely.Application.DTOs;
using Purchasely.Application.Features.Products.Commands;
using Purchasely.Application.Features.Products.Queries;

namespace Purchasely.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default
    )
    {
        var result = await mediator.Send(new GetProductsQuery(page, pageSize), cancellationToken);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductByIdQuery(id), cancellationToken);

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(id, request.SKU, request.Name, request.Description, request.Category);

        var result = await mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteProductCommand(id), cancellationToken);

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return NoContent();
    }
}