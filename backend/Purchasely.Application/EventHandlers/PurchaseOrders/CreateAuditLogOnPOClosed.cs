using MediatR;
using Purchasely.Application.Events.PurchaseOrders;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.EventHandlers.PurchaseOrders;

public class CreateAuditLogOnPOClosed(
    IAuditLogRepository auditRepo
) : INotificationHandler<PurchaseOrderClosedEvent>
{
    public async Task Handle(PurchaseOrderClosedEvent notification, CancellationToken cancellationToken)
    {
        var log = AuditLog.Create(
            "Purchase Order",
            notification.PurchaseOrderId,
            "Purchase order closed",
            notification.CloserId,
            notification.ClosedAt
        );

        await auditRepo.AddAsync(log, cancellationToken);
        await auditRepo.SaveChangesAsync(cancellationToken);
    }
}