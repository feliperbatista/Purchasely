using Purchasely.Domain.Enums;

namespace Purchasely.Application.DTOs;

public record RequisitionResponse(
    Guid Id,
    int Number,
    RequisitionStatus Status,
    Priority Priority,
    string? Justification,
    DateTime? SubmittedAt,
    DateTime CreatedAt,
    string RequesterName,
    IEnumerable<RequisitionLines> Lines
);

public record RequisitionLines(
    Guid Id,
    string ProductName,
    decimal QuantityRequested,
    decimal EstimatedUnitPrice
);