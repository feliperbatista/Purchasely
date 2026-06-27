using MediatR;
using Purchasely.Application.Events.Requisitions;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.EventHandlers.Requisitions;

public class CreateAuditLogOnRequisitionApprovalRemoved(
    IAuditLogRepository auditRepo
) : INotificationHandler<RequisitionApprovalRemovedEvent>
{
    public async Task Handle(RequisitionApprovalRemovedEvent notification, CancellationToken cancellationToken)
    {
        var log = AuditLog.Create(
            "Requisition",
            notification.RequisitionId,
            "Approval removed",
            notification.ApproverId,
            notification.ApprovalRemovedAt
        );

        await auditRepo.AddAsync(log, cancellationToken);
        await auditRepo.SaveChangesAsync(cancellationToken);
    }
}