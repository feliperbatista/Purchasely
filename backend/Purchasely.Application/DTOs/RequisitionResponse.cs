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
    Guid RequesterId,
    IEnumerable<RequisitionLines> Lines,
    IEnumerable<Approvals>? Approvals
);

public record RequisitionLines(
    Guid Id,
    string ProductName,
    decimal QuantityRequested,
    decimal EstimatedUnitPrice
);

public record Approvals(
    Guid Id,
    string Approver,
    DateTime ApprovedAt
);