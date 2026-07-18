namespace Purchasely.Application.DTOs;

public record ProductResponse(
    Guid Id,
    string SKU,
    string Name,
    string? Description,
    string? Category,
    DateTime CreatedAt
);