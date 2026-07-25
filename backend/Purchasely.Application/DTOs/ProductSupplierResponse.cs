namespace Purchasely.Application.DTOs;

public record ProductSupplierResponse(
    Guid SupplierId,
    string SupplierName,
    decimal UnitPrice
);