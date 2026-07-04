using MediatR;

namespace Purchasely.Application.Events.Requisitions;

public record RequisitionApprovedEvent(
    Guid RequisitionId,
    Guid ApprovedById,
    DateTime ApprovedAt,
    int RequisitionNumber,
    string RequesterEmail,
    string RequesterName,
    string ApproverName
) : INotification;

