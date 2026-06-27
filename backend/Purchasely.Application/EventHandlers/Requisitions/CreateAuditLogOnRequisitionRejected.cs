using MediatR;
using Purchasely.Application.Events.Requisitions;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.EventHandlers.Requisitions;

public class CreateAuditLogOnRequisitionRejected(
    IAuditLogRepository auditRepo
) : INotificationHandler<RequisitionRejectedEvent>
{
    public async Task Handle(RequisitionRejectedEvent notification, CancellationToken cancellationToken)
    {
        var log = AuditLog.Create(
            "Requisition",
            notification.RequisitionId,
            "Rejected",
            notification.RejectedById,
            notification.RejectedAt
        );

        await auditRepo.AddAsync(log, cancellationToken);
        await auditRepo.SaveChangesAsync(cancellationToken);
    }
}