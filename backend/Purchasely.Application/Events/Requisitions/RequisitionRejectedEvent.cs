using MediatR;

namespace Purchasely.Application.Events.Requisitions;

public record RequisitionRejectedEvent(
    Guid RequisitionId,
    Guid RequesterId,
    Guid RejectedById,
    string Reason,
    DateTime RejectedAt,
    string RequesterEmail,
    string RequesterName,
    int RequisitionNumber
) : INotification;

