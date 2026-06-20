namespace Purchasely.Application.DTOs;

public record SupplierListResponse(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    bool IsActive
);