namespace Purchasely.Application.DTOs;

public record SupplierDetailsResponse(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    string TaxNumber,
    string Address,
    bool IsActive,
    DateTime CreatedAt,
    IEnumerable<SupplierProducts> Products
);

public record SupplierProducts(
    string SKU,
    string Name,
    decimal UnitPrice,
    string? Description,
    string? Category
);