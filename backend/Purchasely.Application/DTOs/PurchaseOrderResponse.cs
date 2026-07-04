using Purchasely.Domain.Enums;

namespace Purchasely.Application.DTOs;

public record PurchaseOrderResponse(
    Guid Id,
    int PoNumber,
    Guid SupplierId,
    string SupplierName,
    string? PurchasedBy,
    PurchaseOrderStatus Status,
    decimal Subtotal,
    decimal TaxAmount,
    decimal TotalAmount,
    DateTime CreatedAt,
    DateTime? IssuedAt,
    List<PurchaseOrderLineResponse> Lines
);

public record PurchaseOrderLineResponse(
    Guid Id,
    Guid ProductId,
    string ProductName,
    decimal QuantityOrdered,
    decimal? QuantityReceived,
    decimal UnitPrice,
    decimal LineTotal
);