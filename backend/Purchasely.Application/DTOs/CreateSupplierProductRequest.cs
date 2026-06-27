namespace Purchasely.Application.DTOs;

public record CreateSupplierProductRequest(
    Guid ProductId,
    decimal UnitPrice
);