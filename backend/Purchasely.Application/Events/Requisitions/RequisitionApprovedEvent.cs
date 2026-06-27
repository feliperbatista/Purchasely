using MediatR;

namespace Purchasely.Application.Events.Requisitions;

public record RequisitionApprovedEvent(
    Guid RequisitionId,
    Guid ApprovedById,
    DateTime ApprovedAt
) : INotification;

