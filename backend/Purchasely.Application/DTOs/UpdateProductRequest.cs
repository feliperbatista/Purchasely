namespace Purchasely.Application.DTOs;

public record UpdateProductRequest(
    string SKU,
    string Name,
    decimal UnitPrice,
    string? Description
);