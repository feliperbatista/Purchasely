namespace Purchasely.Application.DTOs;

public record SupplierResponse(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    string TaxNumber,
    string Address,
    bool IsActive
);