namespace Purchasely.Application.DTOs;

public record SupplierProductResponse(
    Guid SupplierId,
    Guid ProductId,
    decimal UnitPrice
);