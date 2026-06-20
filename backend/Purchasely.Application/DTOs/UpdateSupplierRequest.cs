namespace Purchasely.Application.DTOs;

public record UpdateSupplierRequest(
    string Name,
    string Email,
    string Phone,
    string TaxNumber,
    string Address
);