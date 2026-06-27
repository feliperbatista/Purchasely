using Purchasely.Domain.Enums;

namespace Purchasely.Application.DTOs;

public record UpdateRequisitionRequest(
    RequisitionStatus Status,
    Priority Priority,
    string? Justification,
    List<RequisitionLinesRequest> Lines
);

public record RequisitionLinesRequest(
    Guid ProductId,
    decimal QuantityRequested,
    decimal EstimatedUnitPrice
);