using MediatR;
using Purchasely.Application.Events.Requisitions;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.EventHandlers.Requisitions;

public class CreateAuditLogOnRequisitionConvertedToPO(
    IAuditLogRepository auditRepo
) : INotificationHandler<RequisitionConvertedToPOEvent>
{
    public async Task Handle(RequisitionConvertedToPOEvent notification, CancellationToken cancellationToken)
    {
        var log = AuditLog.Create(
            "Requisition",
            notification.RequisitionId,
            notification.POIds.Count == 1
                ? $"Converted to PO {notification.POIds.First()}"
                :  $"Converted to POs {string.Join(", ", notification.POIds)}",
            notification.ConvertedBy,
            notification.ConvertedAt
        );

        await auditRepo.AddAsync(log, cancellationToken);
        await auditRepo.SaveChangesAsync(cancellationToken);
    }
}