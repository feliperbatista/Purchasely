namespace Purchasely.Application.DTOs;

public record CreateSupplierRequest(
    string Name,
    string Email,
    string Phone,
    string TaxNumber,
    string Address
);