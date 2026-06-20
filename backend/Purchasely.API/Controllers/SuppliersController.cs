using Microsoft.AspNetCore.Mvc;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController(ISupplierRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var suppliers = await repository.GetAllAsync();

        return Ok(suppliers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var supplier = await repository.GetByIdAsync(id);

        if (supplier is null)
            return NotFound();

        return Ok(supplier);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSupplierRequest request)
    {
        var supplier = Supplier.Create(request.Name, request.Email, request.Phone, request.Address, request.TaxNumber);

        await repository.AddAsync(supplier);
        await repository.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, supplier);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateSupplierRequest request)
    {
        var supplier = await repository.GetByIdAsync(id);

        if (supplier is null)
            return NotFound();

        supplier.Update(request.Name, request.Email, request.Phone, request.Address, request.TaxNumber);

        await repository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var supplier = await repository.GetByIdAsync(id);

        if (supplier is null)
            return NotFound();

        repository.Delete(supplier);

        await repository.SaveChangesAsync();

        return NoContent();
    }
}