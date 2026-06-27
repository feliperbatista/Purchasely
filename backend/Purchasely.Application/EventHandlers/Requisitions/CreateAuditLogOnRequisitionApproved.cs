using MediatR;
using Purchasely.Application.Events.Requisitions;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.EventHandlers.Requisitions;

public class CreateAuditLogOnRequisitionApproved(
    IAuditLogRepository auditRepo
) : INotificationHandler<RequisitionApprovedEvent>
{
    public async Task Handle(RequisitionApprovedEvent notification, CancellationToken cancellationToken)
    {
        var log = AuditLog.Create(
            "Requisition",
            notification.RequisitionId,
            "Approved",
            notification.ApprovedById,
            notification.ApprovedAt
        );

        await auditRepo.AddAsync(log, cancellationToken);
        await auditRepo.SaveChangesAsync(cancellationToken);
    }
}