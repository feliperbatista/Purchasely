using MediatR;
using Purchasely.Application.Events.Requisitions;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.EventHandlers.Requisitions;

public class CreateAuditLogOnRequisitionSubmitted(
    IAuditLogRepository auditRepo
) : INotificationHandler<RequisitionSubmittedEvent>
{
    public async Task Handle(RequisitionSubmittedEvent notification, CancellationToken cancellationToken)
    {
        var log = AuditLog.Create(
            "Requisition",
            notification.RequisitionId,
            "Submitted",
            notification.SubmittedById,
            notification.SubmittedAt
        );

        await auditRepo.AddAsync(log, cancellationToken);
        await auditRepo.SaveChangesAsync(cancellationToken);
    }
}