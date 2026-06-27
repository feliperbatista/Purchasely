namespace Purchasely.Application.Features.Requisitions.Commands;

public record CreateRequisitionLinesCommand(
    Guid ProductId,
    decimal QuantityRequested,
    decimal EstimatedUnitPrice
);