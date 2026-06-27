namespace Purchasely.Application.DTOs;

public record UpdateProductRequest(
    string SKU,
    string Name,
    string? Description,
    string? Category
);