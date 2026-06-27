using MediatR;

namespace Purchasely.Application.Events.Requisitions;

public record RequisitionRejectedEvent(
    Guid RequisitionId,
    Guid RejectedById,
    string Reason,
    DateTime RejectedAt
) : INotification;

