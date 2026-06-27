using MediatR;

namespace Purchasely.Application.Events.Requisitions;

public record RequisitionApprovalRemovedEvent(
    Guid RequisitionId,
    Guid ApproverId,
    DateTime ApprovalRemovedAt
) : INotification;

