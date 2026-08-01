using Purchasely.Domain.Enums;

namespace Purchasely.Application.DTOs;

public record PurchaseOrderResponse(
    Guid Id,
    int PoNumber,
    Guid SupplierId,
    string SupplierName,
    string? CreatedBy,
    PurchaseOrderStatus Status,
    decimal Subtotal,
    decimal TaxAmount,
    decimal TotalAmount,
    string? CancellationReason,
    DateTime CreatedAt,
    DateTime? IssuedAt,
    List<PurchaseOrderLineResponse> Lines,
    List<PurchaseOrderDocumentsResponse>? Documents
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

public record PurchaseOrderDocumentsResponse(
    Guid Id,
    string FileName,
    string ContentType,
    string BlobUrl
);