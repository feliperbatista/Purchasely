namespace Purchasely.Application.DTOs;

public record ProductResponse(
    Guid Id,
    string SKU,
    string Name,
    decimal UnitPrice,
    string? Description,
    Guid SupplierId
);